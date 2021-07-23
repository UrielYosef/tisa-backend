using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL.Repositories
{
    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
        public FlightRepository(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }

        public async Task<IList<Flight>> GetIntersectingFlightsAsync(int airlineId, int airplaneTypeId, DateTime departureTime, DateTime arrivalTime)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            var matchingFlightsByAirline = context.Flights
                .Include(flight => flight.Airplane)
                .Where(flight => flight.Airplane.AirplaneTypeId.Equals(airplaneTypeId) &&
                                 flight.Airplane.AirlineId.Equals(airlineId));

            var matchingFlightsByTimings = await matchingFlightsByAirline
                .Where(flight => (flight.DepartureTime > departureTime && flight.DepartureTime < arrivalTime) ||
                                 (flight.ArrivalTime > departureTime && flight.ArrivalTime < arrivalTime) ||
                                 (flight.DepartureTime < departureTime && flight.ArrivalTime > arrivalTime))
                .ToListAsync();

            return matchingFlightsByTimings;
        }

        public async Task AddDepartmentPricesAsync(IList<DalDepartmentPrice> departmentPrices)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            await context.DepartmentPrices.AddRangeAsync(departmentPrices);
            await context.SaveChangesAsync();
        }
    }
}