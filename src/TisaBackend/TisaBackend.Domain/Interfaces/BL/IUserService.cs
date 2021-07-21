using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Auth;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IUserService
    {
        Task<SignUpResult> RegisterAdminAsync(SignUpModel signUpModel);
        Task<SignInResult> SignInAsync(SignInModel signInModel);
        Task<SignUpResult> SignUpAsync(SignUpModel signUpModel);
        Task<User> FindUserByEmailAsync(string email);
        Task<User> CreateNewUserAsync(string email, string role);
        Task<bool> TryAddUserToAirlineAsync(string userId, int airlineId);
        Task<IList<string>> GetUsersEmails(int airlineId);
    }
}