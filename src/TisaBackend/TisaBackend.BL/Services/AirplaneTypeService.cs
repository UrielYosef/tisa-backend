using System.Collections.Generic;
using System.Threading.Tasks;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{
    public class AirplaneTypeService : IAirplaneTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirplaneTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AirplaneType>> GetAirplaneTypesAsync()
        {
            return await _unitOfWork.AirplaneTypeRepository.GetAllAsync();
        }

        public async Task AddAirplaneTypeAsync(AirplaneType airplaneType)
        {
            await _unitOfWork.AirplaneTypeRepository.AddAsync(airplaneType);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}