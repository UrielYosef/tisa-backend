using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IAirlineRepository : IGenericRepository<Airline>
    {
        Task<Airline> GetAirlineAsync(int airlineId);
    }
}