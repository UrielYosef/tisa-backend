using System;
using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    public class FullyDetailedFight
    {
        public int FlightId { get; set; }
        public string AirlineName { get; set; }
        public string AirplaneType { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Airport SrcAirport{ get; set; }
        public Airport DestAirport { get; set; }
        public IList<DepartmentPrice> DepartmentPrices { get; set; }
        //TODO: add list of departments unoccupied seats 
    }
}