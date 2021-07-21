using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IAirlineService
    {
        Task<IEnumerable<Airline>> GetAllAirlinesAsync();
        Task<IList<AirplaneData>> GetAirlineAirplanesAsync(int airlineId);
        Task UpdateAirplanesAsync(int airlineId, IList<AirplaneData> airplanesData);
        Task<IList<string>> GetAirlineAgentsAsync(int airlineId);
        Task<bool> TryAddAirlineAsync(NewAirlineRequest newAirlineRequest);
        Task<bool> TryAddAirlineAgentAsync(NewAirlineAgentRequest newAirlineAgentRequest);
    }
}