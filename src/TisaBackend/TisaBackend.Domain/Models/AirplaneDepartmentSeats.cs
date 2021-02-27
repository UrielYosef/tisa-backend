namespace TisaBackend.Domain.Models
{
    public class AirplaneDepartmentSeats
    {
        public int Id { get; set; }
        public int SeatsQuantity { get; set; }
        public string AirplaneType { get; set; }
        public string DepartmentType { get; set; }
    }
}