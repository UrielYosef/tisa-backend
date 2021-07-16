using System.Collections.Generic;
using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IAirportRepository : IGenericRepository<Airport>
    {
        //Task<IList<Airport>> GetAirportsAsync(string filter);
    }
}