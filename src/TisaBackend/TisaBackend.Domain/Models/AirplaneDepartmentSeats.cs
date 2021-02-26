namespace TisaBackend.Domain.Models
{
    public class AirplaneDepartmentSeats
    {
        public int SeatsQuantity { get; set; }
        public int AirPlaneTypeId { get; set; }
        public int DepartmentId { get; set; }

        public AirPlaneType AirPlaneType { get; set; }
        public AirplaneDepartmentType AirplaneDepartmentType { get; set; }
    }
}