using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IAirplaneTypeService
    {
        Task<IEnumerable<AirplaneType>> GetAirplaneTypesAsync();
        Task AddAirplaneTypeAsync(AirplaneType airplaneType);
        Task AddDepartmentTypeAsync(DepartmentType departmentType);
        Task AddSeatsToAirplaneTypeDepartmentAsync(AirplaneDepartmentSeats airplaneDepartmentSeats);
    }
}