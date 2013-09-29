using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumc.Models.Response
{
    public class RouteSearch
    {

        public string FromOrigin { get; set; }

        public string ToOrigin { get; set; }
               
        public string FromJson { get; set; }
               
        public string ToJson { get; set; }

        public IEnumerable<RouteStopInformation> FromResults { get; set; }

        public IEnumerable<RouteStopInformation> ToResults { get; set; }
    }
}
