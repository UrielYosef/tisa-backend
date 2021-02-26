using TisaBackend.Domain.Enums;

namespace TisaBackend.Domain.Models
{
    public class AirlineUserRank
    {
        public UserRank Value { get; set; }
        public string Review { get; set; }
        public int UserId { get; set; }
        public int AirlineId { get; set; }

        public Airline Airline { get; set; }
    }
}