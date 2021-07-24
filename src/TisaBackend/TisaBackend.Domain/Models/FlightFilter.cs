using System;

namespace TisaBackend.Domain.Models
{
    public class FlightFilter
    {
        public int SrcAirportId { get; set; }
        public int DestAirportId { get; set; }
        public int NumberOfPassengers { get; set; }
        public DateTime MinDepartureTime { get; set; }
        public DateTime MaxDepartureTime { get; set; }
    }
}