using System.Collections.Generic;
using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IAirportService
    {
        Task<IList<Airport>> GetAirportsAsync(string filter);
        Task AddAirportAsync(Airport airport);
    }
}