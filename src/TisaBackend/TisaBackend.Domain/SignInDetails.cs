using System;

namespace TisaBackend.Domain
{
    public class SignInDetails
    {
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class AirlineSignInDetails : SignInDetails
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
    }
}