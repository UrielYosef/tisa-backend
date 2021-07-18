using System.Threading.Tasks;
using TisaBackend.Domain.Auth;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IUserService
    {
        Task<SignInResult> SignInAsync(SignInModel signInModel);
        Task<SignUpResult> SignUpAsync(SignUpModel signUpModel);
        Task<User> FindUserByEmailAsync(string email);
        Task<User> CreateNewUserAsync(string email);
        Task AddRoleAsync(User user, string role);
    }
}