using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IReportService
    {
        Task<AirlineReportData> GetAirlineReportDataAsync(int airlineId, string username, bool isAdmin);
    }
}