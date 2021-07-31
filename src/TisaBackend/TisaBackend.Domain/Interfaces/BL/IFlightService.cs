using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IFlightService
    {
        Task<FullyDetailedFight> GetFullyDetailedFlightAsync(int flightId);
        Task<IList<NutshellFight>> GetFlightsInANutshellAsync(int airlineId, string username, bool isAdmin);
        Task<IList<NutshellFight>> FilterFlightsAsync(FlightFilter flightFilter);
        Task AddNewFlightAsync(NewFlight newFlight, int airlineId, string username, bool isAdmin);
        Task AddFlightOrderAsync(FlightOrder order, string username);
        Task<IList<NutshellFight>> GetUserFlightsAsync(string username, bool isFuture);
    }
}