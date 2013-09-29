using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sumc.Repositories
{
    public class HttpRequester
    {
        public static HttpWebResponse Get(string resourceUrl, params Cookie[] cookies)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.57 Safari/537.36 OPR/16.0.1196.73";
            request.Accept = "text/html,application/xhtml+xml,application/xml;";

            foreach (var cookie in cookies)
            {
                if (cookie != null)
                {
                    request.CookieContainer.Add(cookie);
                }
            }

            var response = request.GetResponse() as HttpWebResponse;
            return response;
        }

        public static HttpWebResponse Post(string resourceUrl,
            string data,
            params Cookie[] cookies)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.CookieContainer = new CookieContainer();
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.57 Safari/537.36 OPR/16.0.1196.73";
            request.Accept = "text/html,application/xhtml+xml,application/xml;";
            request.AllowAutoRedirect = false;

            foreach (var cookie in cookies)
            {
                if (cookie != null)
                {
                    request.CookieContainer.Add(cookie);
                }
            }


            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
            }

            var response = request.GetResponse() as HttpWebResponse;
            return response;
        }
    }
}