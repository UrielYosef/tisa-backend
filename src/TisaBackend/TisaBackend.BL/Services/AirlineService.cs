using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{
    public class AirlineService : IAirlineService
    {
        private readonly IAirlineRepository _airlineRepository;
        private readonly IRepository<Airplane> _airplaneRepository;
        private readonly IRepository<AirplaneType> _airplaneTypeRepository;
        private readonly IUserService _userService;

        public AirlineService(IAirlineRepository airlineRepository,
            IRepository<Airplane> airplaneRepository,
            IRepository<AirplaneType> airplaneTypeRepository,
            IUserService userService)
        {
            _airlineRepository = airlineRepository;
            _airplaneRepository = airplaneRepository;
            _airplaneTypeRepository = airplaneTypeRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<Airline>> GetAllAirlinesAsync()
        {
            var airlines = await _airlineRepository.GetAllAsync();

            return airlines;
        }

        public async Task<IList<string>> GetAirlineAgentsAsync(int airlineId, string username, bool isAdmin)
        {
            var isAuthorizeForAirline = await _userService.IsAirlineManager(airlineId, username, isAdmin);
            if (!isAuthorizeForAirline)
            {
                throw new ApplicationException("User is not authorize for current airline");
            }

            return await _userService.GetUsersEmails(airlineId);
        }

        public async Task<bool> TryAddAirlineAsync(NewAirlineRequest newAirlineRequest)
        {
            var user = await _userService.FindUserByEmailAsync(newAirlineRequest.AirlineManagerEmail)
                       ?? await _userService.CreateNewUserAsync(newAirlineRequest.AirlineManagerEmail, UserRoles.AirlineManager);
            await _userService.AddRoleToUserAsync(newAirlineRequest.AirlineManagerEmail, UserRoles.AirlineManager);
            var airline = new Airline
            {
                Name = newAirlineRequest.Name,
                AirlineManagerEmail = newAirlineRequest.AirlineManagerEmail
            };

            await _airlineRepository.AddAsync(airline);
            var isSuccess = await _userService.TryAddUserToAirlineAsync(user.Id, airline.Id);

            return isSuccess;
        }

        public async Task<bool> TryAddAirlineAgentAsync(NewAirlineAgentRequest newAirlineAgentRequest, string username, bool isAdmin)
        {
            var isAuthorizeForAirline = await _userService.IsAirlineManager(newAirlineAgentRequest.AirlineId, username, isAdmin);
            if (!isAuthorizeForAirline)
            {
                throw new ApplicationException("User is not authorize for current airline");
            }

            var user = await _userService.FindUserByEmailAsync(newAirlineAgentRequest.Email)
                       ?? await _userService.CreateNewUserAsync(newAirlineAgentRequest.Email, UserRoles.AirlineAgent);
            await _userService.AddRoleToUserAsync(newAirlineAgentRequest.Email, UserRoles.AirlineAgent);

            var isSuccess = await _userService.TryAddUserToAirlineAsync(user.Id, newAirlineAgentRequest.AirlineId);

            return isSuccess;
        }

        public async Task<IList<AirplaneData>> GetAirlineAirplanesAsync(int airlineId, string username, bool isAdmin)
        {
            var isAuthorizeForAirline = await _userService.IsAuthorizeForAirlineAsync(airlineId, username, isAdmin);
            if (!isAuthorizeForAirline)
            {
                throw new ApplicationException("User is not authorize for current airline");
            }

            var airline = await _airlineRepository.GetAirlineAsync(airlineId);
            if (airline is null)
                return null;

            var airplanesTypes = (await _airplaneTypeRepository.GetAllAsync()).ToList();
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

        public async Task UpdateAirplanesAsync(int airlineId, IList<AirplaneData> airplanesData, string username, bool isAdmin)
        {
            var currentAirplanes = await GetAirlineAirplanesAsync(airlineId, username, isAdmin);

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
                    if (airplaneData.Count == existsAirplaneData.Count)
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

            await _airplaneRepository.AddRangeAsync(newAirplanes);
        }
    }
}