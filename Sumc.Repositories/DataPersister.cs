//using HtmlAgilityPack;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sumc.Repositories
//{
//    public class DataPersister
//    {
//        public const string BaseUrl = "http://m.sofiatraffic.bg/";

//        private static string token;
//        private static Cookie authCookie; //vjfmrii (after captcha)
//        private static Cookie mostImportantAuthCookie; //alpocjengi
//        private static Cookie normalCookie; //vjfmrii (every refresh)

//        public async Task<string> GetCaptchaImage()
//        {
//            var response = HttpRequester.Get(BaseUrl + "vt", new Cookie[] { normalCookie, mostImportantAuthCookie, authCookie });
//            this.SetNormalCookie(response);
//            var imgUrl = await await this.GetTextReponse(response);
//            return imgUrl;
//        }

//        public async Task<bool> SetNewSession(string sessionKeyBaby, string code)
//        {
//            if (sessionKeyBaby == "nqmadatikaja" && authCookie == null)
//            {
//                string data = string.Format("q=%D0%B1%D1%8A%D0%BA%D1%81%D1%82%D0%BE%D0%BD&o=1&go=1&poleicngi={0}&sc={1}", token, code);
//                var response = HttpRequester.Post(BaseUrl + "vt", data, new Cookie[] { normalCookie, mostImportantAuthCookie, authCookie });
//                this.SetAuthCookies(response);
//            }

//            return true;
//        }

//        private void SetAuthCookies(HttpWebResponse response)
//        {
//            foreach (Cookie cookie in response.Cookies)
//            {
//                if (cookie.Name == "alpocjengi")
//                {
//                    mostImportantAuthCookie = cookie;
//                    continue;
//                }

//                authCookie = cookie;
//                normalCookie = cookie;
//            }

//            foreach (Cookie cookie in response.Cookies)
//            {
//                if (authCookie != null &&
//                    cookie.Value != authCookie.Value &&
//                    mostImportantAuthCookie != null &&
//                    cookie.Value != mostImportantAuthCookie.Value)
//                {
//                    normalCookie = cookie;
//                }
//            }
//        }

//        private void SetNormalCookie(HttpWebResponse response)
//        {
//            if (response.Cookies.Count != 1)
//            {
//                throw new Exception("Cookies count: " + response.Cookies.Count);
//            }

//            normalCookie = response.Cookies[0];
//        }

//        //private async Task<Task<string>> GetTextReponse(HttpWebResponse response)
//        //{
//        //    return Task.Run<string>(async () =>
//        //    {
//        //        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
//        //        {
//        //            var text = reader.ReadToEnd();
//        //            var imgUrl = await this.GetImageUrl(text);
//        //            return imgUrl;
//        //        }
//        //    });
//        //}

//        //private Task<string> GetImageUrl(string text)
//        //{
//        //    return Task.Run<string>(() =>
//        //    {
//        //        HtmlDocument document = new HtmlDocument();
//        //        document.LoadHtml(text);
//        //        var tokenElement = document.DocumentNode.SelectSingleNode("//input[@name='poleicngi']");
//        //        token = tokenElement.Attributes.First(x => x.Name == "value").Value;

//        //        var imgs = document.DocumentNode.SelectNodes("//img");
//        //        var img = imgs.FirstOrDefault(x => x.Attributes.First(a => a.Name == "src").Value.Contains("captcha"));
//        //        if (img != null)
//        //        {
//        //            var imgUrl = img.Attributes.First(x => x.Name == "src").Value;
//        //            var fullImgUrl = BaseUrl + imgUrl.Substring(1);
//        //            return fullImgUrl;
//        //        }

//        //        return null;
//        //    });
//        //}

//        public string GetSomething(string stage)
//        {
//            var escapedStage = Uri.EscapeUriString(stage);
//            var url = string.Format("{0}vt?q={1}&o=1&go=1", BaseUrl, escapedStage);
//            var response = HttpRequester.Get(url, new Cookie[] { normalCookie, mostImportantAuthCookie, authCookie });
//            this.SetNormalCookie(response);
//            return this.GetTextFromResponse(response);
//        }

//        private string GetTextFromResponse(HttpWebResponse response)
//        {
//            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
//            {
//                var text = reader.ReadToEnd();
//                return text;
//            }
//        }
//    }
//}
