using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IAirplaneTypeRepository : IRepository<AirplaneType>
    {
        Task<IList<DepartmentType>> GetDepartmentTypesAsync(int airplaneTypeId);
        Task AddDepartmentTypeAsync(DepartmentType departmentType);
        Task AddSeatsToAirplaneTypeDepartmentAsync(AirplaneDepartmentSeats airplaneDepartmentSeats);
    }
}