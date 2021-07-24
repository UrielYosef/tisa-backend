using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IFlightService
    {
        Task<FullyDetailedFight> GetFullyDetailedFlightAsync(int flightId);
        Task<IList<NutshellFight>> GetFlightsInANutshellAsync(int airlineId);
        Task<IList<NutshellFight>> FilterFlightsAsync(FlightFilter flightFilter);
        Task AddNewFlightAsync(int airlineId, NewFlight newFlight);
    }
}