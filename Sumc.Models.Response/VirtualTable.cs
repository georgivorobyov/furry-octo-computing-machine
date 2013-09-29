using System.Collections.Generic;

namespace Sumc.Models.Response
{
    public class VirtualTable
    {
        public VirtualTable()
        {

        }

        public VirtualTable(IEnumerable<VehicleVirtualTable> buses, IEnumerable<VehicleVirtualTable> trolleys, IEnumerable<VehicleVirtualTable> trams)
        {
            this.Buses = buses;
            this.Trolleys = trolleys;
            this.Trams = trams;
        }

        public bool IsFound { get; set; }

        public Stop Stop { get; set; }

        public string Error { get; set; }

        public string Information { get; set; }

        public IEnumerable<Stop> FoundStops { get; set; }

        public IEnumerable<VehicleVirtualTable> Buses { get; set; }

        public IEnumerable<VehicleVirtualTable> Trolleys { get; set; }

        public IEnumerable<VehicleVirtualTable> Trams { get; set; }
    }
}