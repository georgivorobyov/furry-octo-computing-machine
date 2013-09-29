using HtmlAgilityPack;
using Newtonsoft.Json;
using Sumc.Repositories;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sumc.WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        private Cookie sessionKey;
        private Cookie globalAuthKey;

        protected readonly SessionRepository sessionRepository;

        public BaseApiController()
        {
            this.sessionRepository = new SessionRepository();
        }

        public Cookie SessionKey
        {
            get
            {
                if (this.sessionKey == null)
                {
                    this.SetSessions();
                }

                return this.sessionKey;
            }

            protected set
            {
                if (this.sessionKey != value)
                {
                    this.sessionRepository.SetSession(value);
                    this.sessionKey = value;
                }
            }
        }

        public Cookie GlobalAuthKey
        {
            get
            {
                if (this.globalAuthKey == null)
                {
                    this.SetSessions();
                }

                return this.globalAuthKey;
            }

            protected set
            {
                this.globalAuthKey = value;
            }
        }

        private void SetSessions()
        {
            var session = this.sessionRepository.GetSession();
            if (session.IsActive)
            {
                this.globalAuthKey = JsonConvert.DeserializeObject<Cookie>(session.GlobalAuthToken);
                //throw new ArgumentOutOfRangeException("Cookie");
            }

            this.sessionKey = JsonConvert.DeserializeObject<Cookie>(session.SessionToken);
        }

        protected HtmlDocument GetDocumentReader(string text)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(text);
            return document;
        }

        protected string GetTextFromResponse(HttpWebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var text = reader.ReadToEnd();
                return text;
            }
        }

        protected Cookie GetCookie(HttpWebResponse response)
        {
            if (response.Cookies.Count > 1)
            {
                this.sessionRepository.DeactivateAuthCookie();
                throw new ArgumentOutOfRangeException("Cookie");
            }

            return response.Cookies["vjfmrii"];
        }

        protected string UrlEncode(string stringForEncode)
        {
            return stringForEncode.Replace("&", "%26");
        }

        protected string UrlDecode(string stringForDecode)
        {
            return stringForDecode.Replace("%26", "&");
        }

        protected HttpResponseMessage PerformOperation(Action action)
        {
            try
            {
                action();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                    new HttpError("Auth"));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Possible Pars Error");
            }
        }

        protected HttpResponseMessage PerformOperation<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                    new HttpError("Auth"));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Possible Pars Error");
            }
        }
    }
}