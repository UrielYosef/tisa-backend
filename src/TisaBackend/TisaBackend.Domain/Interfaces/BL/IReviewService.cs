using System.Threading.Tasks;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IReviewService
    {
        Task<AirlineReviewData> GetAirlineReviewsAsync(int airlineId);
        Task AddReviewAsync(Review review);
    }
}