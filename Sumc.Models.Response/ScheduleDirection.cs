using System.Collections.Generic;

namespace Sumc.Models.Response
{
    public class ScheduleDirection
    {
        public string Direction { get; set; }

        public IEnumerable<RouteStopInformation> Stops { get; set; }
    }
}
