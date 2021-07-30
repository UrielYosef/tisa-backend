using System;
using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    public class NewFlight
    {
        public int AirplaneTypeId { get; set; }
        public int SrcAirportId { get; set; }
        public int DestAirportId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public IList<DepartmentData> DepartmentPrices { get; set; }
    }
}