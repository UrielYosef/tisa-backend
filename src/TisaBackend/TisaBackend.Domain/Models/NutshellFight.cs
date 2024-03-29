﻿using System;

namespace TisaBackend.Domain.Models
{
    public class NutshellFight
    {
        public int FlightId { get; set; }
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string AirplaneType { get; set; }
        public int MinimalPrice { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Airport SrcAirport{ get; set; }
        public Airport DestAirport { get; set; }
    }
}