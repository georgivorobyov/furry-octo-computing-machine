using System.Collections.Generic;

namespace Sumc.Models.Response
{
    public class ScheduleVirtualTable : Vehicle
    {
        public string VehicleType { get; set; }

        public bool IsFound { get; set; }

        public Stop Stop { get; set; }

        public string Error { get; set; }

        public string Information { get; set; }

        public IEnumerable<string> RightTimes { get; set; }

        public IEnumerable<VehicleVirtualTable> Buses { get; set; }

        public IEnumerable<VehicleVirtualTable> Trolleys { get; set; }

        public IEnumerable<VehicleVirtualTable> Trams { get; set; }

        public new string ArrivalsTimes { get; set; }
    }
}