using System.Threading.Tasks;

namespace TisaBackend.Domain.Interfaces
{
    public interface IUserService
    {
        Task<string> ProvideAirlineManagerUser(string email);
    }
}