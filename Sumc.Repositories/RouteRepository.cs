using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sumc.Repositories
{
    public class RouteRepository : BaseRequestRepository
    {
        public HttpWebResponse GetRoutes(string from, string to, params Cookie[] cookies)
        {
            var url = BaseSofiaTrafficUrl + "bg/pt/search";
            var data = string.Format("from={0}&to={1}", from, to);
            return HttpRequester.Post(url, data, cookies);
        }

        public HttpWebResponse GetRoutes(string from, string to, string fromJson, string toJson, int chosenTo, int chosenFrom, string fromOrigin, string toOrigin, params Cookie[] cookies)
        {
            var url = BaseSofiaTrafficUrl + "bg/pt/search";
            var data = string.Format("from={0}&to={1}&json_from={2}&json_to={3}&orig_from={4}&orig_to={5}", from, to, fromJson, toJson, fromOrigin, toOrigin);
            return HttpRequester.Post(url, data, cookies);
        }
    }
}
