using System;
using System.Threading.Tasks;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAirportRepository AirportRepository { get; set; }
        IRepository<AirplaneType> AirplaneTypeRepository { get; set; }
        IRepository<DepartmentType> DepartmentTypeRepository { get; set; }
        IRepository<AirplaneDepartmentSeats> AirplaneDepartmentSeatsRepository { get; set; }

        IAirlineRepository AirlineRepository { get; set; }
        IRepository<Airplane> AirplaneRepository { get; set; }
        IFlightRepository FlightRepository { get; set; }

        Task<int> SaveChangesAsync();
    }
}