using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IAirportRepository : IRepository<Airport>
    {
        Task<IList<Airport>> GetAirportsAsync(string filter);
    }
}