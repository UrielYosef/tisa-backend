using System.Threading.Tasks;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAirportRepository AirportRepository { get; set; }
        public IGenericRepository<AirplaneType> AirplaneTypeRepository { get; set; }
        public IGenericRepository<DepartmentType> DepartmentTypeRepository { get; set; }
        public IGenericRepository<AirplaneDepartmentSeats> AirplaneDepartmentSeatsRepository { get; set; }

        public IAirlineRepository AirlineRepository { get; set; }
        public IGenericRepository<Airplane> AirplaneRepository { get; set; }
        public IFlightRepository FlightRepository { get; set; }

        private readonly TisaContext _context;

        public UnitOfWork(TisaContext context,
            IGenericRepository<AirplaneType> airplaneTypeRepository,
            IGenericRepository<DepartmentType> departmentTypeRepository,
            IGenericRepository<AirplaneDepartmentSeats> airplaneDepartmentSeatsRepository,
            IGenericRepository<Airplane> airplaneRepository,
            IAirlineRepository airlineRepository,
            IAirportRepository airportRepository,
            IFlightRepository flightRepository)
        { 
            _context = context;

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
            return await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}