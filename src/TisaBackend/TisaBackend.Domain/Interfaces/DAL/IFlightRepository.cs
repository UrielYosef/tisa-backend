using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<IList<Flight>> GetFlightsAsync(int airlineId);
        Task<IList<Flight>> GetIntersectingFlightsAsync(int airlineId, int airplaneTypeId, DateTime departureTime, DateTime arrivalTime);
        Task AddDepartmentPricesAsync(IList<DalDepartmentPrice> departmentPrices);
    }
}