using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAirportRepository AirportRepository { get; set; }
        public IRepository<AirplaneType> AirplaneTypeRepository { get; set; }
        public IRepository<DepartmentType> DepartmentTypeRepository { get; set; }
        public IRepository<AirplaneDepartmentSeats> AirplaneDepartmentSeatsRepository { get; set; }

        public IAirlineRepository AirlineRepository { get; set; }
        public IRepository<Airplane> AirplaneRepository { get; set; }
        public IFlightRepository FlightRepository { get; set; }

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UnitOfWork(IServiceScopeFactory serviceScopeFactory,
            IRepository<AirplaneType> airplaneTypeRepository,
            IRepository<DepartmentType> departmentTypeRepository,
            IRepository<AirplaneDepartmentSeats> airplaneDepartmentSeatsRepository,
            IRepository<Airplane> airplaneRepository,
            IAirlineRepository airlineRepository,
            IAirportRepository airportRepository,
            IFlightRepository flightRepository)
        {
            _serviceScopeFactory = serviceScopeFactory;

            AirplaneTypeRepository = airplaneTypeRepository;
            DepartmentTypeRepository = departmentTypeRepository;
            AirplaneDepartmentSeatsRepository = airplaneDepartmentSeatsRepository;

            AirlineRepository = airlineRepository;
            AirplaneRepository = airplaneRepository;
            FlightRepository = flightRepository;
            AirportRepository = airportRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            return await context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            await context.DisposeAsync();
        }
    }
}