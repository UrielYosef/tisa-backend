using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IAirlineRepository : IGenericRepository<Airline>
    {
        Task<Airline> GetAirlineAsync(int airlineId);
    }
}