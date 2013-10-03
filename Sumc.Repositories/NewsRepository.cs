using Sumc.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sumc.Repositories
{
    public class NewsRepository :BaseRequestRepository
    {
        public HttpWebResponse GetNews(Cookie sessionToken, Cookie authToken)
        {
            var url = string.Format("{0}bg/news/p/1", BaseSofiaTrafficUrl);
            return HttpRequester.Get(url, sessionToken, authToken);
        }
        //id == real url
        public HttpWebResponse GetNews(string id, Cookie sessionToken, Cookie authToken)
        {//891/avtobusna-liniia-107
            var url = string.Format("{0}bg/news/{1}", BaseSofiaTrafficUrl, id);
            return HttpRequester.Get(url, sessionToken, authToken);
        }
    }
}
