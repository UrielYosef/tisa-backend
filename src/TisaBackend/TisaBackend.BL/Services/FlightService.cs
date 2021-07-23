using System;
using System.Linq;
using System.Threading.Tasks;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IAirlineRepository _airlineRepository;

        public FlightService(IFlightRepository flightRepository, IAirlineRepository airlineRepository)
        {
            _flightRepository = flightRepository;
            _airlineRepository = airlineRepository;
        }

        //TODO: add api for getting flights by airlineId

        public async Task AddNewFlightAsync(int airlineId, NewFlight newFlight)
        {
            var airline = await _airlineRepository.GetAirlineAsync(airlineId);
            var intersectingFlights = await _flightRepository
                .GetIntersectingFlightsAsync(airlineId, newFlight.AirplaneTypeId, newFlight.DepartureTime, newFlight.ArrivalTime);
            if (intersectingFlights.Count >= airline.Airplanes.Count)
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
    }
}