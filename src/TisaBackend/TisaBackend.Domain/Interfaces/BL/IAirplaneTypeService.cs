using System.Collections.Generic;
using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IAirplaneTypeService
    {
        Task<IEnumerable<AirplaneType>> GetAirplaneTypesAsync();
        Task AddAirplaneTypeAsync(AirplaneType airplaneType);
    }
}