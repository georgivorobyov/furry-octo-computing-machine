using HtmlAgilityPack;
using Sumc.Models;
using Sumc.Models.Response;
using Sumc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sumc.WebApi.Controllers
{
    public class SessionsController : BaseApiController
    {
        private readonly AdminRepository adminRepository;

        public SessionsController()
        {
            this.adminRepository = new AdminRepository();
        }

        public AuthForm GetCaptcha()
        {
            if (this.sessionRepository.IsActiveAuthCookie())
            {
                throw new InvalidOperationException();
            }

            var response = this.sessionRepository.GetVirtualTableResponse();
            var text = this.GetTextFromResponse(response);
            var document = this.GetDocumentReader(text);
            var tokenElement = document.DocumentNode.SelectSingleNode("//input[@name='poleicngi']");
            if (tokenElement == null) throw new ArgumentNullException("token");

            var formToken = tokenElement.Attributes.First(x => x.Name == "value").Value;
            var cookie = this.GetCookie(response);
            this.sessionRepository.SetSession(cookie);
            //this.sessionRepository.SetFormTokenAndSession(formToken, cookie);
            var captcha = this.GetPictureUrl(document);
            if (captcha == null) throw new ArgumentNullException("captcha");
            var authForm = new AuthForm
            {
                CaptchaUrl = captcha,
                Token = formToken
            };
            return authForm;
        }

        public bool Authenticate(string token, string code)
        {
            if (this.sessionRepository.IsActiveAuthCookie())
            {
                throw new InvalidOperationException();
            }

            var response = this.sessionRepository.PostCaptchaCode(token, code);
            this.SetCookies(response);
            var locationHeader = response.Headers.AllKeys.FirstOrDefault(x => x == "Location");
            //var text = this.GetTextFromResponse(response);
            //var document = this.GetDocumentReader(text);
            //var imgNode = this.GetPictureUrl(document);
            //return imgNode == null;
            var locationHeaderExists = locationHeader != null;
            return locationHeaderExists;
        }

        private void SetCookies(HttpWebResponse response)
        {
            Cookie authCookie = null;
            Cookie sessionCookie = null;
            foreach (Cookie cookie in response.Cookies)
            {
                if (cookie.Name == "alpocjengi")
                {
                    authCookie = cookie;
                    continue;
                }

                sessionCookie = cookie;
            }

            this.sessionRepository.SetAuthCookie(authCookie, sessionCookie);
        }

        private string GetPictureUrl(HtmlDocument document)
        {
            var imgs = document.DocumentNode.SelectNodes("//img");
            var img = imgs.FirstOrDefault(x => x.Attributes.First(a => a.Name == "src").Value.Contains("captcha"));
            if (img != null)
            {
                var imgUrl = img.Attributes.First(x => x.Name == "src").Value;
                var fullImgUrl = SessionRepository.BaseSofiaTrafficUrl + imgUrl.Substring(1);
                return fullImgUrl;
            }

            return null;
        }

        private bool IsAdmin(string absoluteSecretKey)
        {
            var crypted = GenerateSaltedSha1(absoluteSecretKey);
            var admin = this.adminRepository.GetAdminByPassword(crypted);
            return admin == null ? false : true;
        }

        #region Sha1
        private static string GenerateSaltedSha1(string plainTextString)
        {
            HashAlgorithm algorithm = new SHA1Managed();
            var saltBytes = GenerateSalt(4);
            var plainTextBytes = Encoding.ASCII.GetBytes(plainTextString);

            var plainTextWithSaltBytes = AppendByteArray(plainTextBytes, saltBytes);
            var saltedSha1Bytes = algorithm.ComputeHash(plainTextWithSaltBytes);
            var saltedSha1WithAppendedSaltBytes = AppendByteArray(saltedSha1Bytes, saltBytes);

            return "{SSHA}" + Convert.ToBase64String(saltedSha1WithAppendedSaltBytes);
        }

        private static byte[] GenerateSalt(int saltSize)
        {
            var buff = new byte[saltSize];
            for (int i = 0; i < saltSize; i++)
            {
                buff[i] = (byte)(i % 27);
            }

            return buff;
        }

        private static byte[] AppendByteArray(byte[] byteArray1, byte[] byteArray2)
        {
            var byteArrayResult =
                    new byte[byteArray1.Length + byteArray2.Length];

            for (var i = 0; i < byteArray1.Length; i++)
                byteArrayResult[i] = byteArray1[i];
            for (var i = 0; i < byteArray2.Length; i++)
                byteArrayResult[byteArray1.Length + i] = byteArray2[i];

            return byteArrayResult;
        }
        #endregion
    }
}