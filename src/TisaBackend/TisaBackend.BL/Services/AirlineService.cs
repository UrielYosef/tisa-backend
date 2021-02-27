using System.Threading.Tasks;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{
    public class AirlineService : IAirlineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirlineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAirlineAsync(Airline airline)
        {
            await _unitOfWork.AirlineRepository.AddAsync(airline);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddAirplaneAsync(Airplane airplane)
        {
            await _unitOfWork.AirplaneRepository.AddAsync(airplane);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}