using System.Threading.Tasks;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TisaContext _context;

        public IAirportRepository AirportRepository { get; set; }
        public IGenericRepository<AirplaneType> AirplaneTypeRepository { get; set; }
        public IGenericRepository<AirplaneDepartmentType> AirplaneDepartmentTypeRepository { get; set; }
        public IGenericRepository<AirplaneDepartmentSeats> AirplaneDepartmentSeatsRepository { get; set; }


        public IAirlineRepository AirlineRepository { get; set; }
        public IGenericRepository<Airplane> AirplaneRepository { get; set; }
        public IFlightRepository FlightRepository { get; set; }

        public UnitOfWork(TisaContext context, IAirportRepository airportRepository)
        { 
            _context = context;

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