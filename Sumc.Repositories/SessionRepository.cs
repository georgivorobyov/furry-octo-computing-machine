using System.Net;
using Sumc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sumc.Repositories
{
    public class SessionRepository : BaseRequesterRepository<Session>
    {
        private Session cachedSession;

        public Session GetSession()
        {
            if (this.cachedSession == null)
            {
                this.cachedSession = this.dbSet.FirstOrDefault();
            }

            return this.cachedSession;
        }

        public void SetFormTokenAndSession(string token, Cookie cookie)
        {
            var session = this.GetSession();
            session.FormToken = token;
            session.SessionToken = this.CookieToString(cookie);
            this.SaveChanges();
        }

        public void SetAuthCookie(Cookie cookie, Cookie sessionCookie)
        {
            var session = this.GetSession();
            session.SessionToken = this.CookieToString(sessionCookie);
            if (cookie != null)
            {
                session.GlobalAuthToken = this.CookieToString(cookie);
                session.IsActive = true;
            }
            this.SaveChanges();
        }

        public void DeactivateAuthCookie()
        {
            var session = this.GetSession();
            session.IsActive = false;
            this.SaveChanges();
        }

        public bool IsActiveAuthCookie()
        {
            var session = this.GetSession();
            return session.IsActive;
        }

        public HttpWebResponse GetVirtualTableResponse()
        {
            return HttpRequester.Get(BaseSofiaTrafficUrl + "vt");
        }

        public HttpWebResponse PostCaptchaCode(string code)
        {
            var session = this.GetSession();
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
            var session = this.GetSession();
            session.SessionToken = JsonConvert.SerializeObject(cookie);
            this.SaveChanges();
        }

        public HttpWebResponse PostCaptchaCode(string token, string code)
        {
            var session = this.GetSession();
            string data = string.Format("q=%D0%B1%D1%8A%D0%BA%D1%81%D1%82%D0%BE%D0%BD&o=1&go=1&poleicngi={0}&sc={1}", token, code);
            var cookie = JsonConvert.DeserializeObject<Cookie>(session.SessionToken);
            var response = HttpRequester.Post(BaseSofiaTrafficUrl + "vt", data, cookie);
            return response;
        }
    }
}
