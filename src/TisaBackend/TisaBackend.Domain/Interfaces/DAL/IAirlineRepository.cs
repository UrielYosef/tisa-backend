using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IAirlineRepository : IRepository<Airline>
    {
        Task<Airline> GetAirlineAsync(int airlineId);
    }
}