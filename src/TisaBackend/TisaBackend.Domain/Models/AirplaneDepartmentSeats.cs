namespace TisaBackend.Domain.Models
{
    public class AirplaneDepartmentSeats
    {
        public int Id { get; set; }
        public int AirplaneTypeId { get; set; }
        public int DepartmentTypeId { get; set; }
        public int SeatsQuantity { get; set; }

        public AirplaneType AirplaneType { get; set; }
        public DepartmentType DepartmentType { get; set; }
    }
}