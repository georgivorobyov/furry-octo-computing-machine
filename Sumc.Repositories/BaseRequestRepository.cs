using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sumc.Repositories
{
    public class BaseRequestRepository
    {
        public const string BaseSofiaTrafficUrl = "http://m.sofiatraffic.bg/";

        protected string GetTextFromResponse(HttpWebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var text = reader.ReadToEnd();
                return text;
            }
        }
    }
}
