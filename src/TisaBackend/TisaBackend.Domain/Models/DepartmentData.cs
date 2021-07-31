namespace TisaBackend.Domain.Models
{
    public class DepartmentData
    {
        public int DepartmentId { get; set; }
        public string DisplayName { get; set; }
        public int Price { get; set; }
        public int Seats { get; set; }
        public int AvailableSeats { get; set; }
    }
}