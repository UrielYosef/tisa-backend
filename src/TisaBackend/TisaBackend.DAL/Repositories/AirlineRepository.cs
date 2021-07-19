using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL.Repositories
{
    public class AirlineRepository : Repository<Airline>, IAirlineRepository
    {
        public AirlineRepository(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }

        public async Task<Airline> GetAirlineAsync(int airlineId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            return await context.Airlines
                .Include(airline => airline.Airplanes)
                .ThenInclude(airplane => airplane.AirplaneType)
                .Where(airline => airline.Id.Equals(airlineId))
                .SingleOrDefaultAsync();
        }
    }
}