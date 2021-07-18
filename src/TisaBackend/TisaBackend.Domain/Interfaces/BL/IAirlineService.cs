using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IAirlineService
    {
        Task<Airline> GetAirlineByUserIdAsync(string userId);
        Task<IEnumerable<Airline>> GetAllAirlinesAsync();
        Task<IList<AirplaneData>> GetAirlineAirplanesAsync(int airlineId);
        Task UpdateAirplanesAsync(int airlineId, IList<AirplaneData> airplanesData);
        Task AddAirlineAsync(NewAirlineRequest newAirlineRequest);
        Task AddAirlineAgentAsync(NewAirlineAgentRequest newAirlineAgentRequest);
    }
}