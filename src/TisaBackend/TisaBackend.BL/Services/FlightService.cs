using System;
using System.Collections.Generic;
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

        public async Task<IList<NutshellFight>> GetFlightsInANutshellAsync(int airlineId)
        {
            var nutshellFlights = new List<NutshellFight>();

            var airlineFlights = await _flightRepository.GetFlightsAsync(airlineId);
            foreach (var airlineFlight in airlineFlights)
            {
                var minimalPrice = airlineFlight.DepartmentPrices.Min(deptPrice => deptPrice.Price);
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

        public async Task<IList<NutshellFight>> FilterFlightsAsync(FlightFilter flightFilter)
        {
            var nutshellFlights = new List<NutshellFight>();

            var airlineFlights = await _flightRepository.GetFlightsAsync(flightFilter);
            foreach (var airlineFlight in airlineFlights)
            {
                var minimalPrice = airlineFlight.DepartmentPrices.Min(deptPrice => deptPrice.Price);
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
    }
}