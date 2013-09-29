using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sumc.Repositories
{
    public class VirtualTableRepository : BaseRequestRepository
    {
        public HttpWebResponse Get(string query, params Cookie[] cookies)
        {
            var url = string.Format("{0}vt?q={1}&go=1&o=1", BaseSofiaTrafficUrl, query);
            return HttpRequester.Get(url, cookies);
        }
    }
}
