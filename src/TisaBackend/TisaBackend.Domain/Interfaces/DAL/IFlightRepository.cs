using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<Flight> GetFlightAsync(int flightId);
        Task<IList<Flight>> GetFlightsByAirlineAsync(int airlineId);
        Task<IList<Flight>> GetFlightsByUserAsync(string userId, bool isFuture);
        Task<IList<Flight>> GetFlightsAsync(FlightFilter flightFilter);
        Task<IList<Flight>> GetIntersectingFlightsAsync(int airlineId, int airplaneTypeId, DateTime departureTime, DateTime arrivalTime);
        Task AddDepartmentPricesAsync(IList<DalDepartmentPrice> departmentPrices);
        Task AddFlightOrderAsync(FlightOrder order);
        Task<int> GetDepartmentFlightOrdersAsync(int flightId, int departmentId);
    }
}