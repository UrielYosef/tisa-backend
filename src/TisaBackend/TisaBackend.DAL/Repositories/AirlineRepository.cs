using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL.Repositories
{
    public class AirlineRepository : Repository<Airline>, IAirlineRepository
    {
        public AirlineRepository(TisaContext context) : base(context)
        {

        }

        public async Task<Airline> GetAirlineAsync(int airlineId)
        {
            return await Context.Airlines
                .Include(airline => airline.Airplanes)
                .ThenInclude(airplane => airplane.AirplaneType)
                .Where(airline => airline.Id.Equals(airlineId))
                .SingleOrDefaultAsync();
        }
    }
}