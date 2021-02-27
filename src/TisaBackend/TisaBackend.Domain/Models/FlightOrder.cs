namespace TisaBackend.Domain.Models
{
    public class FlightOrder
    {
        public int Id { get; set; }
        public int Username { get; set; }
        public int FlightId { get; set; }
        public int DepartmentId { get; set; }
        public int SeatsQuantity { get; set; }

        public Flight Flight { get; set; }
        public AirplaneDepartmentType AirplaneDepartmentType { get; set; }
    }
}