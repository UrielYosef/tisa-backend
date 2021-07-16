using Microsoft.AspNetCore.Identity;

namespace TisaBackend.DAL.Auth
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}