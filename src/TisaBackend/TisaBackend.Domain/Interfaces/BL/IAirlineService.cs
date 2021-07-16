using System.Collections.Generic;
using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IAirlineService
    {
        Task<IEnumerable<Airline>> GetAllAirlinesAsync();
        Task<IDictionary<string, int>> GetAirlineAirplanesAsync(int airlineId);
        Task AddAirlineAsync(Airline airline);
        Task AddAirplaneAsync(Airplane airplane);
    }
}