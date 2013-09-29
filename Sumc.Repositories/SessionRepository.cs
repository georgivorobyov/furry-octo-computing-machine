using System.Net;
using Sumc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sumc.Data;

namespace Sumc.Repositories
{
    public class SessionRepository : BaseRequestRepository
    {
        private Session cachedSession;

        public Session GetSession()
        {
            if (this.cachedSession == null)
            {
                using (var context = new SumcContext())
                {
                    this.cachedSession = context.Sessions.FirstOrDefault();
                }
            }

            return this.cachedSession;
        }

        public void SetFormTokenAndSession(string token, Cookie cookie)
        {
            using (var context = new SumcContext())
            {
                var session = context.Sessions.FirstOrDefault();
                session.FormToken = token;
                session.SessionToken = this.CookieToString(cookie);
                context.SaveChanges();
            }
        }

        public void SetAuthCookie(Cookie cookie, Cookie sessionCookie)
        {
            using (var context = new SumcContext())
            {
                var session = context.Sessions.FirstOrDefault();
                session.SessionToken = this.CookieToString(sessionCookie);
                if (cookie != null)
                {
                    session.GlobalAuthToken = this.CookieToString(cookie);
                    session.IsActive = true;
                }

                context.SaveChanges();
            }
        }

        public void DeactivateAuthCookie()
        {
            using (var context = new SumcContext())
            {
                var session = context.Sessions.FirstOrDefault();
                session.IsActive = false;

                context.SaveChanges();
            }
        }

        public bool IsActiveAuthCookie()
        {
            using (var context = new SumcContext())
            {
                var session = context.Sessions.FirstOrDefault();
                return session.IsActive;
            }
        }

        public HttpWebResponse GetVirtualTableResponse()
        {
            return HttpRequester.Get(BaseSofiaTrafficUrl + "vt");
        }

        public HttpWebResponse PostCaptchaCode(string code)
        {
            Session session = null;
            using (var context = new SumcContext())
            {
                session = context.Sessions.FirstOrDefault();
            }

            string data = string.Format("q=%D0%B1%D1%8A%D0%BA%D1%81%D1%82%D0%BE%D0%BD&o=1&go=1&poleicngi={0}&sc={1}", session.FormToken, code);
            var cookie = JsonConvert.DeserializeObject<Cookie>(session.SessionToken);
            var response = HttpRequester.Post(BaseSofiaTrafficUrl + "vt", data, cookie);
            return response;
        }

        private string CookieToString(Cookie cookie)
        {
            return JsonConvert.SerializeObject(cookie);
        }

        public void SetSession(Cookie cookie)
        {
            using (var context = new SumcContext())
            {
                var session = context.Sessions.FirstOrDefault();
                session.SessionToken = JsonConvert.SerializeObject(cookie);
                context.SaveChanges();
            }
        }

        public HttpWebResponse PostCaptchaCode(string token, string code)
        {
            Session session = null;
            using (var context = new SumcContext())
            {
                session = context.Sessions.FirstOrDefault();
            }

            string data = string.Format("q=%D0%B1%D1%8A%D0%BA%D1%81%D1%82%D0%BE%D0%BD&o=1&go=1&poleicngi={0}&sc={1}", token, code);
            var cookie = JsonConvert.DeserializeObject<Cookie>(session.SessionToken);
            var response = HttpRequester.Post(BaseSofiaTrafficUrl + "vt", data, cookie);
            return response;
        }
    }
}
