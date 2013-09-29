using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumc.Models.Response
{
    public class Schedule : Vehicle
    {
        //vehicletype is possible moved to Vehicle class, but I'm not sure for now.
        public TransportType VehicleType { get; set; }

        public Stop Stop { get; set; }
        
        public string Error { get; set; }

        public string RightTimeUrl { get; set; }

        public IEnumerable<ExtendedStop> Stops { get; set; }

        public bool IsFound { get; set; }
    }
}
