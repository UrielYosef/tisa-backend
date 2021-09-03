using System;
using System.Linq;
using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    public class AirlineReportData
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public IList<FlightReportData> FlightsData { get; set; }
        public int? AverageOccupancyPercentage => CalculateAverageOccupancyPercentage();
        public int? TotalOfIncome => FlightsData?.Sum(data => data.TotalOfIncome);

        private int? CalculateAverageOccupancyPercentage()
        {
            if (!FlightsData?.Any() ?? true)
                return null;

            return (int?)Math.Round((decimal)FlightsData.Average(data => data.OccupancyPercentage), 0);
        }
    }
}