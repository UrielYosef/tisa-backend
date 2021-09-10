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

        public async Task<Flight> GetFlightAsync(int flightId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            var matchingFlight = await context.Flights
                .Include(flight => flight.Airplane)
                    .ThenInclude(airplane => airplane.AirplaneType)
                .Include(flight => flight.Airplane)
                    .ThenInclude(airplane => airplane.Airline)
                .Include(flight => flight.DepartmentPrices)
                    .ThenInclude(departmentPrice => departmentPrice.Department)
                .Include(flight => flight.SrcAirport)
                .Include(flight => flight.DestAirport)
                .SingleOrDefaultAsync(flight => flight.Id.Equals(flightId));

            return matchingFlight;
        }

        public async Task<IList<Flight>> GetFlightsByAirlineAsync(int airlineId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            var matchingFlightsByAirline = await context.Flights
                .Include(flight => flight.Airplane)
                    .ThenInclude(airplane => airplane.AirplaneType)
                .Include(flight => flight.Airplane)
                    .ThenInclude(airplane => airplane.Airline)
                .Include(flight => flight.DepartmentPrices)
                    .ThenInclude(departmentPrice => departmentPrice.Department)
                .Include(flight => flight.SrcAirport)
                .Include(flight => flight.DestAirport)
                .Where(flight => flight.Airplane.AirlineId.Equals(airlineId))
                .ToListAsync();

            return matchingFlightsByAirline;
        }

        public async Task<IList<Flight>> GetFlightsByUserAsync(string userId, bool isFuture)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            var flightIds = await context.FlightOrders
                .Include(flightOrder => flightOrder.Flight)
                .Where(flightOrder => flightOrder.UserId.Equals(userId) &&
                                      ((isFuture && flightOrder.Flight.DepartureTime > DateTime.Now) ||
                                      (!isFuture && flightOrder.Flight.DepartureTime < DateTime.Now)))
                .Select(flightOrder => flightOrder.FlightId)
                .Distinct()
                .ToListAsync();

            var matchingFlightsByIds = await context.Flights
                .Include(flight => flight.Airplane)
                    .ThenInclude(airplane => airplane.AirplaneType)
                .Include(flight => flight.Airplane)
                    .ThenInclude(airplane => airplane.Airline)
                .Include(flight => flight.DepartmentPrices)
                    .ThenInclude(departmentPrice => departmentPrice.Department)
                .Include(flight => flight.SrcAirport)
                .Include(flight => flight.DestAirport)
                .Where(flight => flightIds.Contains(flight.Id))
                .ToListAsync();

            return matchingFlightsByIds;
        }

        public async Task<IList<Flight>> GetFlightsAsync(FlightFilter flightFilter)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            var matchingFlights = context.Flights
                .Include(flight => flight.Airplane)
                    .ThenInclude(airplane => airplane.AirplaneType)
                .Include(flight => flight.Airplane)
                    .ThenInclude(airplane => airplane.Airline)
                .Include(flight => flight.DepartmentPrices)
                    .ThenInclude(departmentPrice => departmentPrice.Department)
                .Include(flight => flight.SrcAirport)
                .Include(flight => flight.DestAirport)
                .Where(flight => flightFilter.MinDepartureTime <= flight.DepartureTime);

            if (flightFilter.MaxDepartureTime != default)
                matchingFlights = matchingFlights
                    .Where(flight => flightFilter.MaxDepartureTime >= flight.DepartureTime);
            if(!flightFilter.SrcAirportId.Equals(-1))
                matchingFlights = matchingFlights
                    .Where(flight => flight.SrcAirportId.Equals(flightFilter.SrcAirportId));
            if (!flightFilter.DestAirportId.Equals(-1))
                matchingFlights = matchingFlights
                    .Where(flight => flight.DestAirportId.Equals(flightFilter.DestAirportId));

            return await matchingFlights.ToListAsync();
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

        public async Task AddFlightOrderAsync(FlightOrder order)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            await context.FlightOrders.AddRangeAsync(order);
            await context.SaveChangesAsync();
        }

        public async Task<IList<FlightOrder>> GetFlightsOrdersAsync(int airlineId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            var orders = await context.FlightOrders
                .Include(order => order.User)
                .Include(order => order.Flight)
                    .ThenInclude(flight => flight.Airplane)
                        .ThenInclude(airplane => airplane.Airline)
                .Include(order => order.Flight)
                       .ThenInclude(flight => flight.SrcAirport)
                .Include(order => order.Flight)
                    .ThenInclude(flight => flight.DestAirport)
                .Include(order => order.Flight)
                    .ThenInclude(flight => flight.DepartmentPrices)
                        .ThenInclude(departmentPrice => departmentPrice.Department)
                .Where(order => order.Flight.Airplane.AirlineId.Equals(airlineId))
                .ToListAsync();

            return orders;
        }

        public async Task<int> GetDepartmentFlightOrdersAsync(int flightId, int departmentId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            return await context.FlightOrders
                .Where(order => order.FlightId.Equals(flightId) &&
                                order.DepartmentId.Equals(departmentId))
                .SumAsync(order => order.SeatsQuantity);
        }
    }
}