using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IAirlineService
    {
        Task<IEnumerable<Airline>> GetAllAirlinesAsync();
        Task<IList<AirplaneData>> GetAirlineAirplanesAsync(int airlineId, string username, bool isAdmin);
        Task UpdateAirplanesAsync(int airlineId, IList<AirplaneData> airplanesData, string username, bool isAdmin);
        Task<IList<string>> GetAirlineAgentsAsync(int airlineId, string username, bool isAdmin);
        Task<bool> TryAddAirlineAsync(NewAirlineRequest newAirlineRequest);
        Task<bool> TryAddAirlineAgentAsync(NewAirlineAgentRequest newAirlineAgentRequest, string username, bool isAdmin);
    }
}