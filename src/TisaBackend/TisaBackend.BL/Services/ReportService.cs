﻿using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;
using TisaBackend.Domain.Interfaces.BL;

namespace TisaBackend.BL.Services
{
    public class ReportService : IReportService
    {
        private readonly IAirlineService _airlineService;
        private readonly IFlightService _flightService;

        public ReportService(IAirlineService airlineService, IFlightService flightService)
        {
            _airlineService = airlineService;
            _flightService = flightService;
        }

        public async Task<IList<AirlineReportData>> GetAirlinesReportsDataAsync(string username, bool isAdmin)
        {
            var airlines = (await _airlineService.GetAllAirlinesAsync())?.ToList();
            if (!airlines?.Any() ?? true)
                return null;

            var reports = new List<AirlineReportData>();
            foreach (var airline in airlines)
            {
                var report = await GetAirlineReportDataAsync(airline.Id, username, isAdmin);
                reports.Add(report);
            }

            return reports;
        }

        public async Task<AirlineReportData> GetAirlineReportDataAsync(int airlineId, string username, bool isAdmin)
        {
            var nutshellFights = await _flightService.GetFlightsInANutshellAsync(airlineId, username, isAdmin);
            if (!nutshellFights?.Any() ?? true)
                return null;

            var airlineName = nutshellFights.First().AirlineName;
            var flights = await GetFullyFlightsDetailsAsync(nutshellFights.Select(flight => flight.FlightId));
            var flightsData = CalculateFlightsData(flights);

            var report = new AirlineReportData
            {
                AirlineId = airlineId,
                AirlineName = airlineName,
                FlightsData = flightsData
            };

            return report;
        }

        private async Task<IList<FullyDetailedFight>> GetFullyFlightsDetailsAsync(IEnumerable<int> flightIds)
        {
            var flights = new List<FullyDetailedFight>();
            foreach (var flightId in flightIds)
            {
                var flight = await _flightService.GetFullyDetailedFlightAsync(flightId);

                flights.Add(flight);
            }

            return flights;
        }

        private IList<FlightReportData> CalculateFlightsData(IList<FullyDetailedFight> flights)
        {
            var flightsData = new List<FlightReportData>();

            foreach (var flight in flights)
            {
                var flightData = new FlightReportData
                {
                    FlightId = flight.FlightId,
                    OccupancyPercentage = flight.DepartmentsData
                        .Select(departmentData => (double) (departmentData.Seats - departmentData.AvailableSeats) / departmentData.Seats)
                        .Average(),
                    TotalOfIncome = flight.DepartmentsData
                        .Select(departmentData => (departmentData.Seats - departmentData.AvailableSeats) * departmentData.Price)
                        .Sum()
                };

                flightsData.Add(flightData);
            }

            return flightsData;
        }
    }
}