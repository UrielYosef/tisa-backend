using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Airline>> GetAllAirlinesAsync()
        {
            var airlines = await _unitOfWork.AirlineRepository.GetAllAsync();

            return airlines;
        }

        public async Task<IDictionary<string, int>> GetAirlineAirplanesAsync(int airlineId)
        {
            var airline =  await _unitOfWork.AirlineRepository.GetAirlineAsync(airlineId);
            var airPlaneTypesToAmount = airline.Airplanes
                .GroupBy(airplane => airplane.AirplaneType.Name)
                .ToDictionary(groupedAirplane => groupedAirplane.Key,
                    groupedAirplane => groupedAirplane.Count());


            return airPlaneTypesToAmount;
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