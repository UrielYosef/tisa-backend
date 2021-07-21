using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL.Repositories
{
    public class AirportRepository : Repository<Airport>, IAirportRepository
    {
        public AirportRepository(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }

        public async Task<IList<Airport>> GetAirportsAsync(string filter)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            return await context.Airports
                .Where(airport => airport.AlphaCode.ToLower().Contains(filter) ||
                                  airport.Name.ToLower().Contains(filter) ||
                                  airport.Country.ToLower().Contains(filter) ||
                                  airport.City.ToLower().Contains(filter))
                .ToListAsync();
        }
    }
}