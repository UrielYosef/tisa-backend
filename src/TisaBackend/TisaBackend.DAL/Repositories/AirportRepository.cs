using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL.Repositories
{
    public class AirportRepository : GenericRepository<Airport>, IAirportRepository
    {
        public AirportRepository(TisaContext context) : base(context)
        {

        }

        public async Task<IList<Airport>> GetAirportsAsync(string filter)
        {
            return await Context.Airports
                .Where(airport => airport.AlphaCode.ToLower().Contains(filter) ||
                                  airport.Name.ToLower().Contains(filter) ||
                                  airport.Country.ToLower().Contains(filter) ||
                                  airport.City.ToLower().Contains(filter))
                .ToListAsync();
        }
    }
}