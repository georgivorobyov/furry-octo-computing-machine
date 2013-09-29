using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumc.Models.Response
{
    public class Vehicle
    {
        public IEnumerable<string> ArrivalsTimes { get; set; }

        public string Title { get; set; }

        public string Route { get; set; }
    }
}
