using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    public class AirlineOrdersReportData
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public IList<FlightOrderReportData> FlightsOrdersData { get; set; }
    }
}