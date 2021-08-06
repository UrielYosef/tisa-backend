using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IReviewService
    {
        Task<IList<Review>> GetAirlineReviewsAsync(int airlineId);
        Task AddReviewAsync(Review review);
    }
}