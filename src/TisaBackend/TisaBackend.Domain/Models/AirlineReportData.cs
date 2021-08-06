using System.Linq;
using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    public class AirlineReportData
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public IList<FlightReportData> FlightsData { get; set; }
        public double? AverageOccupancyPercentage => FlightsData?.Average(data => data.OccupancyPercentage);
        public int? TotalOfIncome => FlightsData?.Sum(data => data.TotalOfIncome);
    }

    public class FlightReportData
    {
        public int FlightId { get; set; }
        public int TotalOfIncome { get; set; }
        public double OccupancyPercentage { get; set; }
    }
}