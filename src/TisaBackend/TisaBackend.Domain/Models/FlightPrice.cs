namespace TisaBackend.Domain.Models
{
    public class FlightPrice
    {
        public int PriceInDollars { get; set; }
        public int FlightId { get; set; }
        public int DepartmentId { get; set; }

        public Flight Flight { get; set; }
        public AirplaneDepartmentType AirplaneDepartmentType { get; set; }
    }
}