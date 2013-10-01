using System.Collections.Generic;

namespace Sumc.Models.Response
{
    public class MainStop : Stop
    {
        public IEnumerable<string> TransportNumbers { get; set; }
    }
}
