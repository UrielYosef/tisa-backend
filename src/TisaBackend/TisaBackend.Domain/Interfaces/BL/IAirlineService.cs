using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IAirlineService
    {
        Task AddAirlineAsync(Airline airline);
        Task AddAirplaneAsync(Airplane airplane);
    }
}