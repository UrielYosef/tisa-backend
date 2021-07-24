using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRepository;

        public AirportService(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository;
        }

        public async Task<IList<Airport>> GetAirportsAsync(string filter)
        {
            return await _airportRepository.GetAirportsAsync(filter?.ToLower() ?? string.Empty);
        }

        public async Task AddAirportAsync(Airport airport)
        {
            await _airportRepository.AddAsync(airport);
        }
    }
}