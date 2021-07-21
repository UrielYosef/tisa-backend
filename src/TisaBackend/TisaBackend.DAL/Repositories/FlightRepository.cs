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
    }
}