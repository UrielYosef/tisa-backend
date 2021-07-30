using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IAirlineRepository _airlineRepository;
        private readonly IAirplaneTypeRepository _airplaneTypeRepository;
        private readonly IUserService _userService;

        public FlightService(IUserService userService,
            IFlightRepository flightRepository,
            IAirlineRepository airlineRepository,
            IAirplaneTypeRepository airplaneTypeRepository)
        {
            _userService = userService;
            _flightRepository = flightRepository;
            _airlineRepository = airlineRepository;
            _airplaneTypeRepository = airplaneTypeRepository;
        }

        public async Task<FullyDetailedFight> GetFullyDetailedFlightAsync(int flightId)
        {
            var dalFlight = await _flightRepository.GetFlightAsync(flightId);
            if (dalFlight is null)
                return null;

            var fullyDetailedFlight = new FullyDetailedFight
            {
                FlightId = flightId,
                AirlineName = dalFlight.Airplane.Airline.Name,
                AirplaneType = dalFlight.Airplane.AirplaneType.Name,
                DepartureTime = dalFlight.DepartureTime,
                ArrivalTime = dalFlight.ArrivalTime,
                SrcAirport = dalFlight.SrcAirport,
                DestAirport = dalFlight.DestAirport,
                DepartmentPrices = new List<DepartmentPrice>(),
                DepartmentIdToUnoccupiedSeats = new Dictionary<int, int>()
            };

            foreach (var departmentPrice in dalFlight.DepartmentPrices)
            {
                fullyDetailedFlight.DepartmentPrices.Add(new DepartmentPrice
                {
                    DepartmentId = departmentPrice.DepartmentId,
                    DisplayName = departmentPrice.Department.Name,
                    Price = departmentPrice.Price
                });

                var unoccupiedSeats = await GetUnoccupiedSeatsAsync(flightId, departmentPrice.DepartmentId);
                fullyDetailedFlight.DepartmentIdToUnoccupiedSeats
                    .Add(departmentPrice.DepartmentId, unoccupiedSeats);
            }

            return fullyDetailedFlight;
        }

        public async Task<IList<NutshellFight>> GetFlightsInANutshellAsync(int airlineId)
        {
            var nutshellFlights = new List<NutshellFight>();

            var airlineFlights = await _flightRepository.GetFlightsAsync(airlineId);
            foreach (var airlineFlight in airlineFlights)
            {
                var minimalPrice = airlineFlight.DepartmentPrices.Min(deptPrice => deptPrice.Price);
                var nutshellFlight = new NutshellFight
                {
                    FlightId = airlineFlight.Id,
                    MinimalPrice = minimalPrice,
                    AirlineName = airlineFlight.Airplane.Airline.Name,
                    AirplaneType = airlineFlight.Airplane.AirplaneType.Name,
                    DepartureTime = airlineFlight.DepartureTime,
                    ArrivalTime = airlineFlight.ArrivalTime,
                    SrcAirport = airlineFlight.SrcAirport,
                    DestAirport = airlineFlight.DestAirport
                };

                nutshellFlights.Add(nutshellFlight);
            }
            
            return nutshellFlights;
        }

        public async Task<IList<NutshellFight>> FilterFlightsAsync(FlightFilter flightFilter)
        {
            var nutshellFlights = new List<NutshellFight>();

            var airlineFlights = await _flightRepository.GetFlightsAsync(flightFilter);
            foreach (var airlineFlight in airlineFlights)
            {
                var minimalPrice = airlineFlight.DepartmentPrices.Min(deptPrice => deptPrice.Price);
                var minimalPriceDepartmentId = airlineFlight.DepartmentPrices
                    .Where(departmentPrice => departmentPrice.Price.Equals(minimalPrice))
                    .Select(departmentPrice => departmentPrice.DepartmentId)
                    .FirstOrDefault();
                var unoccupiedSeats = await GetUnoccupiedSeatsAsync(airlineFlight.Id, minimalPriceDepartmentId);
                if(unoccupiedSeats < flightFilter.NumberOfPassengers)
                    continue;

                var nutshellFlight = new NutshellFight
                {
                    MinimalPrice = minimalPrice,
                    AirlineName = airlineFlight.Airplane.Airline.Name,
                    AirplaneType = airlineFlight.Airplane.AirplaneType.Name,
                    DepartureTime = airlineFlight.DepartureTime,
                    ArrivalTime = airlineFlight.ArrivalTime,
                    SrcAirport = airlineFlight.SrcAirport,
                    DestAirport = airlineFlight.DestAirport
                };

                nutshellFlights.Add(nutshellFlight);
            }

            return nutshellFlights;
        }

        public async Task AddNewFlightAsync(int airlineId, NewFlight newFlight)
        {
            var airline = await _airlineRepository.GetAirlineAsync(airlineId);
            var intersectingFlights = await _flightRepository
                .GetIntersectingFlightsAsync(airlineId, newFlight.AirplaneTypeId,
                    newFlight.DepartureTime, newFlight.ArrivalTime);
            var airplanesOfCurrentTypesCount = airline.Airplanes
                .Count(airplane => airplane.AirplaneTypeId.Equals(newFlight.AirplaneTypeId));
            if (intersectingFlights.Count >= airplanesOfCurrentTypesCount)
            {
                throw new ApplicationException("Airline does not have enough airplanes for this flight");
            }

            var intersectingFlightsAirplanesIds = intersectingFlights
                .Select(flight => flight.AirplaneId)
                .ToList();

            var availableAirplaneId = airline.Airplanes
                .Where(airplane => airplane.AirplaneTypeId.Equals(newFlight.AirplaneTypeId))
                .Select(airplane => airplane.Id)
                .First(airplaneId=> !intersectingFlightsAirplanesIds.Contains(airplaneId));

            var dalFlight = new Flight
            {
                DepartureTime = newFlight.DepartureTime,
                ArrivalTime = newFlight.ArrivalTime,
                SrcAirportId = newFlight.SrcAirportId,
                DestAirportId = newFlight.DestAirportId,
                AirplaneId = availableAirplaneId,
            };

            await _flightRepository.AddAsync(dalFlight);

            var dalDepartmentPrices = newFlight.DepartmentPrices
                .Select(departmentPrice =>
                    new DalDepartmentPrice(dalFlight.Id, departmentPrice.DepartmentId, departmentPrice.Price))
                .ToList();

            await _flightRepository.AddDepartmentPricesAsync(dalDepartmentPrices);
        }

        public async Task AddFlightOrderAsync(FlightOrder order, string username)
        {
            var userId = await _userService.GetUserIdByUsername(username);
            order.UserId = userId;
            var unoccupiedSeats = await GetUnoccupiedSeatsAsync(order.FlightId, order.DepartmentId);
            if (unoccupiedSeats < order.SeatsQuantity)
            {
                throw new ApplicationException("Flight does not have enough seats at this department");
            }

            await _flightRepository.AddFlightOrderAsync(order);
        }

        private async Task<int> GetUnoccupiedSeatsAsync(int flightId, int departmentId)
        {
            var flight = await _flightRepository.GetFlightAsync(flightId);
            var airplaneTypeId = flight.Airplane.AirplaneTypeId;
            var departmentSeats = await _airplaneTypeRepository.GetSeatsQuantityAsync(airplaneTypeId, departmentId);

            var flightOrders = await _flightRepository.GetDepartmentFlightOrdersAsync(flightId, departmentId);

            return departmentSeats - flightOrders;
        }
    }
}