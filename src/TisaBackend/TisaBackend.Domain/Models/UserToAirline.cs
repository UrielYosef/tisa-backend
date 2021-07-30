using TisaBackend.Domain.Models.Auth;

namespace TisaBackend.Domain.Models
{
    public class UserToAirline
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AirlineId { get; set; }

        public User User { get; set; }
        public Airline Airline { get; set; }

        public UserToAirline()
        {
            
        }

        public UserToAirline(string userId, int airlineId)
        {
            UserId = userId;
            AirlineId = airlineId;
        }
    }
}