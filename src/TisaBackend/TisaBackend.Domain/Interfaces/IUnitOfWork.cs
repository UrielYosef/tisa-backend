using System;
using System.Threading.Tasks;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAirportRepository AirportRepository { get; set; }
        IGenericRepository<AirplaneType> AirplaneTypeRepository { get; set; }
        IGenericRepository<AirplaneDepartmentType> AirplaneDepartmentTypeRepository { get; set; }
        IGenericRepository<AirplaneDepartmentSeats> AirplaneDepartmentSeatsRepository { get; set; }


        IAirlineRepository AirlineRepository { get; set; }
        IGenericRepository<Airplane> AirplaneRepository { get; set; }
        IFlightRepository FlightRepository { get; set; }

        Task<int> SaveChangesAsync();
    }
}