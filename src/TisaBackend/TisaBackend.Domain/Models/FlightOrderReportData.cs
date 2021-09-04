namespace TisaBackend.Domain.Models
{
    public class FlightOrderReportData
    {
        public int FlightId { get; set; }
        public string Username { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string DepartureDate { get; set; }
        public string DepartmentName { get; set; }
        public int SeatsQuantity { get; set; }
    }
}