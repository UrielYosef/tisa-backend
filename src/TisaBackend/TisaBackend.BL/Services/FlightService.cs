using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
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
                AirlineId = dalFlight.Airplane.AirlineId,
                AirlineName = dalFlight.Airplane.Airline.Name,
                AirplaneType = dalFlight.Airplane.AirplaneType.Name,
                DepartureTime = dalFlight.DepartureTime,
                ArrivalTime = dalFlight.ArrivalTime,
                SrcAirport = dalFlight.SrcAirport,
                DestAirport = dalFlight.DestAirport,
                DepartmentsData = new List<DepartmentData>(),
            };

            foreach (var departmentPrice in dalFlight.DepartmentPrices)
            {
                var seatsAndUnoccupiedSeats = await GetSeatsAndUnoccupiedSeatsAsync(flightId, departmentPrice.DepartmentId);
                fullyDetailedFlight.DepartmentsData.Add(new DepartmentData
                {
                    DepartmentId = departmentPrice.DepartmentId,
                    DisplayName = departmentPrice.Department.Name,
                    Price = departmentPrice.Price,
                    Seats = seatsAndUnoccupiedSeats.Seats,
                    AvailableSeats = seatsAndUnoccupiedSeats.UnoccupiedSeats
                });
            }

            return fullyDetailedFlight;
        }

        public async Task<IList<NutshellFight>> GetFlightsInANutshellAsync(int airlineId, string username, bool isAdmin)
        {
            var isAuthorizeForAirline = await _userService.IsAuthorizeForAirlineAsync(airlineId, username, isAdmin);
            if (!isAuthorizeForAirline)
            {
                throw new ApplicationException("User is not authorize for current airline");
            }

            var airlineFlights = await _flightRepository.GetFlightsByAirlineAsync(airlineId);

            var nutshellFlights = ParseFlights(airlineFlights);

            return nutshellFlights;
        }

        public async Task<IList<NutshellFight>> FilterFlightsAsync(FlightFilter flightFilter)
        {
            var airlineFlights = (await _flightRepository.GetFlightsAsync(flightFilter)).ToList();
            var notEnoughSpaceflightsId = new List<int>();
            foreach (var airlineFlight in airlineFlights)
            {
                var minimalPrice = airlineFlight.DepartmentPrices.Min(deptPrice => deptPrice.Price);
                var minimalPriceDepartmentId = airlineFlight.DepartmentPrices
                    .Where(departmentPrice => departmentPrice.Price.Equals(minimalPrice))
                    .Select(departmentPrice => departmentPrice.DepartmentId)
                    .FirstOrDefault();
                var seatsAndUnoccupiedSeats = await GetSeatsAndUnoccupiedSeatsAsync(airlineFlight.Id, minimalPriceDepartmentId);
                if (seatsAndUnoccupiedSeats.UnoccupiedSeats < flightFilter.NumberOfPassengers)
                    notEnoughSpaceflightsId.Add(airlineFlight.Id);
            }

            var suitableFlights = airlineFlights
                .Where(airlineFlight => !notEnoughSpaceflightsId.Contains(airlineFlight.Id))
                .ToList();

            var nutshellFlights = ParseFlights(suitableFlights);

            return nutshellFlights;
        }

        public async Task AddNewFlightAsync(NewFlight newFlight, int airlineId, string username, bool isAdmin)
        {
            var isAuthorizeForAirline = await _userService.IsAuthorizeForAirlineAsync(airlineId, username, isAdmin);
            if (!isAuthorizeForAirline)
            {
                throw new ApplicationException("User is not authorize for current airline");
            }

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
            var userId = await _userService.GetUserIdByUsernameAsync(username);
            order.UserId = userId;
            var seatsAndUnoccupiedSeats = await GetSeatsAndUnoccupiedSeatsAsync(order.FlightId, order.DepartmentId);
            if (seatsAndUnoccupiedSeats.UnoccupiedSeats < order.SeatsQuantity)
            {
                throw new ApplicationException("Flight does not have enough seats at this department");
            }

            await _flightRepository.AddFlightOrderAsync(order);
        }

        public async Task<IList<NutshellFight>> GetUserFlightsAsync(string username, bool isFuture)
        {
            var userId = await _userService.GetUserIdByUsernameAsync(username);
            if (string.IsNullOrEmpty(userId))
                return null;

            var flights = await _flightRepository.GetFlightsByUserAsync(userId, isFuture);

            var nutshellFlights = ParseFlights(flights);

            return nutshellFlights;
        }

        public async Task<IList<FlightOrder>> GetFlightsOrdersAsync(int airlineId, string username, bool isAdmin)
        {
            var isAuthorizeForAirline = await _userService.IsAuthorizeForAirlineAsync(airlineId, username, isAdmin);
            if (!isAuthorizeForAirline)
            {
                throw new ApplicationException("User is not authorize for current airline");
            }

            var orders = await _flightRepository.GetFlightsOrdersAsync(airlineId);

            return orders;
        }

        private IList<NutshellFight> ParseFlights(IList<Flight> flights)
        {
            var nutshellFlights = new List<NutshellFight>();

            foreach (var flight in flights)
            {
                var minimalPrice = flight.DepartmentPrices.Min(deptPrice => deptPrice.Price);
                var nutshellFlight = new NutshellFight
                {
                    FlightId = flight.Id,
                    MinimalPrice = minimalPrice,
                    AirlineId = flight.Airplane.AirlineId,
                    AirlineName = flight.Airplane.Airline.Name,
                    AirplaneType = flight.Airplane.AirplaneType.Name,
                    DepartureTime = flight.DepartureTime,
                    ArrivalTime = flight.ArrivalTime,
                    SrcAirport = flight.SrcAirport,
                    DestAirport = flight.DestAirport
                };

                nutshellFlights.Add(nutshellFlight);
            }

            return nutshellFlights;
        }

        private async Task<(int Seats, int UnoccupiedSeats)> GetSeatsAndUnoccupiedSeatsAsync(int flightId, int departmentId)
        {
            var flight = await _flightRepository.GetFlightAsync(flightId);
            var airplaneTypeId = flight.Airplane.AirplaneTypeId;
            var departmentSeats = await _airplaneTypeRepository.GetSeatsQuantityAsync(airplaneTypeId, departmentId);

            var flightOrders = await _flightRepository.GetDepartmentFlightOrdersAsync(flightId, departmentId);

            return (departmentSeats, departmentSeats - flightOrders);
        }
    }
}