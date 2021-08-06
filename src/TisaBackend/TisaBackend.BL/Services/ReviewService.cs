using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{ 
    public class ReviewService : IReviewService
    {
        private readonly IUserService _userService;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IUserService userService, IReviewRepository reviewRepository)
        {
            _userService = userService;
            _reviewRepository = reviewRepository;
        }

        public async Task<IList<Review>> GetAirlineReviewsAsync(int airlineId)
        {
            var reviews = new List<Review>();
            var dalReviews = await _reviewRepository.GetAirlineReviews(airlineId);
            foreach (var dalReview in dalReviews)
            {
                var review = await ParseDalReviewAsync(dalReview);

                reviews.Add(review);
            }

            return reviews;
        }

        public async Task AddReviewAsync(Review review)
        {
            var dalReview = await ParseReviewAsync(review);

            await _reviewRepository.AddAsync(dalReview);
        }

        private async Task<Review> ParseDalReviewAsync(DalReview dalReview)
        {
            var username = (await _userService.FindUserByUserIdAsync(dalReview.UserId)).UserName;

            return new Review
            {
                AirlineId = dalReview.AirlineId,
                Username = username,
                Headline = dalReview.Headline,
                Content = dalReview.Content,
                Ranking = dalReview.Ranking,
                ReviewDate = dalReview.ReviewDate
            };
        }

        private async Task<DalReview> ParseReviewAsync(Review review)
        {
            var userId = (await _userService.FindUserByUsernameAsync(review.Username)).Id;

            return new DalReview
            {
                AirlineId = review.AirlineId,
                UserId = userId,
                Headline = review.Headline,
                Content = review.Content,
                Ranking = review.Ranking,
                ReviewDate = review.ReviewDate
            };
        }
    }
}