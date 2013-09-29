using System.Collections.Generic;

namespace Sumc.Models.Response
{
    public class ScheduleDirections : ResponseBase
    {
        public IEnumerable<ScheduleDirection> Directions { get; set; }

        public string Lid { get; set; }

        public string Rid { get; set; }

        public string TransportNumber { get; set; }

        public TransportType TransportType { get; set; }
    }
}