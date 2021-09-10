using TisaBackend.Domain.Models.Auth;

namespace TisaBackend.Domain.Models
{
    public class FlightOrder
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FlightId { get; set; }
        public int DepartmentId { get; set; }
        public int SeatsQuantity { get; set; }
        
        public Flight Flight { get; set; }
        public User User { get; set; }
    }
}