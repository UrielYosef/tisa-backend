using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IFlightService
    {
        Task AddNewFlightAsync(int airlineId, NewFlight newFlight);
    }
}