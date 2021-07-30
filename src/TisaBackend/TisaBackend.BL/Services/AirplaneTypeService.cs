using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{
    public class AirplaneTypeService : IAirplaneTypeService
    {
        private readonly IAirplaneTypeRepository _airplaneTypeRepository;

        public AirplaneTypeService(IAirplaneTypeRepository airplaneTypeRepository)
        {
            _airplaneTypeRepository = airplaneTypeRepository;
        }

        public async Task<IList<DepartmentType>> GetDepartmentTypesAsync(int airplaneTypeId)
        {
            return await _airplaneTypeRepository.GetDepartmentTypesAsync(airplaneTypeId);
        }

        public async Task<IEnumerable<AirplaneType>> GetAirplaneTypesAsync()
        {
            return await _airplaneTypeRepository.GetAllAsync();
        }

        public async Task AddAirplaneTypeAsync(AirplaneType airplaneType)
        {
            await _airplaneTypeRepository.AddAsync(airplaneType);
        }

        public async Task AddDepartmentTypeAsync(DepartmentType departmentType)
        {
            await _airplaneTypeRepository.AddDepartmentTypeAsync(departmentType);
        }

        public async Task AddSeatsToAirplaneTypeDepartmentAsync(AirplaneDepartmentSeats airplaneDepartmentSeats)
        {
            await _airplaneTypeRepository.AddSeatsToAirplaneTypeDepartmentAsync(airplaneDepartmentSeats);
        }
    }
}