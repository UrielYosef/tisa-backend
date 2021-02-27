﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int SrcAirportId { get; set; }
        public int DestAirportId { get; set; }
        public string AirplaneType { get; set; }
        public IList<FlightPrice> FlightPrices { get; set; }

        public Airport SrcAirport { get; set; }
        public Airport DestAirport { get; set; }
        public Airplane Airplane { get; set; }
    }
}