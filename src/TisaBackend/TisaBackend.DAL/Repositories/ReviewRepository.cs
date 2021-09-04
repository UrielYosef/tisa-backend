using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL.Repositories
{
    public class ReviewRepository : Repository<DalReview>, IReviewRepository
    {
        public ReviewRepository(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }

        public async Task<IList<DalReview>> GetAirlineReviews(int airlineId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            return await context.Reviews
                .Include(review => review.Airline)
                .Where(review => review.AirlineId.Equals(airlineId))
                .ToListAsync();
        }
    }
}