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
        private readonly IUserService _userService;

        public AirlineService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<IEnumerable<Airline>> GetAllAirlinesAsync()
        {
            var airlines = await _unitOfWork.AirlineRepository.GetAllAsync();

            return airlines;
        }

        public async Task<IList<AirplaneData>> GetAirlineAirplanesAsync(int airlineId)
        {
            var airline = await _unitOfWork.AirlineRepository.GetAirlineAsync(airlineId);
            if (airline is null)
                return null;

            var airplanesTypes = (await _unitOfWork.AirplaneTypeRepository.GetAllAsync()).ToList();
            var airplanesData = airplanesTypes
                .Select(airplaneType => new AirplaneData
                {
                    Id = airplaneType.Id,
                    Name = airplaneType.Name,
                    Count = 0
                })
                .ToList();

            var airplaneTypesToAmount = airline.Airplanes
                .GroupBy(airplane => airplane.AirplaneType.Name)
                .ToDictionary(groupedAirplane => groupedAirplane.Key,
                    groupedAirplane => groupedAirplane.Count());

            foreach (var (airplaneType, airplaneTypeAmount) in airplaneTypesToAmount)
            {
                var currentAirplaneData = airplanesData
                    .Single(airplaneData => airplaneData.Name.Equals(airplaneType));
                currentAirplaneData.Count = airplaneTypeAmount;
            }

            return airplanesData;
        }

        public async Task AddAirlineAsync(NewAirlineRequest newAirlineRequest)
        {
            var airlineManagerUserName =
                await _userService.ProvideAirlineManagerUser(newAirlineRequest.AirlineManagerEmail);

            var airline = new Airline
            {
                Name = newAirlineRequest.Name,
                AirlineManagerUser = airlineManagerUserName
            };

            await _unitOfWork.AirlineRepository.AddAsync(airline);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAirplanesAsync(int airlineId, IList<AirplaneData> airplanesData)
        {
            var currentAirplanes = await GetAirlineAirplanesAsync(airlineId);

            foreach (var airplaneData in airplanesData)
            {
                var existsAirplaneData = currentAirplanes
                    .SingleOrDefault(airplaneD => airplaneD.Id.Equals(airplaneData.Id));
                if (existsAirplaneData is null)
                {
                    await AddAirplanesAsync(airplaneData.Count, airlineId, airplaneData.Id);
                }
                else
                {
                    if(airplaneData.Count == existsAirplaneData.Count)
                        continue;
                    else if (airplaneData.Count > existsAirplaneData.Count)
                    {
                        await AddAirplanesAsync(airplaneData.Count - existsAirplaneData.Count, airlineId, airplaneData.Id);
                    }
                    else
                    {
                        //TODO: implement - if we delete, we need to get also zero planes data
                        continue;
                    }
                }
            }
        }

        private async Task AddAirplanesAsync(int count, int airlineId, int airplaneTypeId)
        {
            var newAirplanes = Enumerable
                .Range(1, count)
                .Select(index => new Airplane
                {
                    AirlineId = airlineId,
                    AirplaneTypeId = airplaneTypeId
                });

            await _unitOfWork.AirplaneRepository.AddRangeAsync(newAirplanes);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}