using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.DAL
{
    public interface IReviewRepository : IRepository<DalReview>
    {
        Task<IList<DalReview>> GetAirlineReviews(int airlineId);
    }
}