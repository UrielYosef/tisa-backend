using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{
    public class AirportService : IAirportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<Airport>> GetAirportsAsync(string filter)
        {
            return await _unitOfWork.AirportRepository.GetAirportsAsync(filter?.ToLower() ?? string.Empty);
        }

        public async Task AddAirportAsync(Airport airport)
        {
            await _unitOfWork.AirportRepository.AddAsync(airport);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}