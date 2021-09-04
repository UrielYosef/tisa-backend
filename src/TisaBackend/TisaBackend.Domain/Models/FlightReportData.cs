using System;

namespace TisaBackend.Domain.Models
{
    public class FlightReportData
    {
        public int FlightId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureDate { get; set; }
        public int TotalOfIncome { get; set; }
        public int OccupancyPercentage { get; set; }
    }
}