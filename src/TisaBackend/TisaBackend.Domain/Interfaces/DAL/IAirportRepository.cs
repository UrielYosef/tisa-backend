using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IAirportRepository : IGenericRepository<Airport>
    {
        Task<IList<Airport>> GetAirportsAsync(string filter);
    }
}