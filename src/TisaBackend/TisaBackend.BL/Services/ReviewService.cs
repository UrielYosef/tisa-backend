﻿using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.BL.Services
{ 
    public class ReviewService : IReviewService
    {
        private readonly IUserService _userService;
        private readonly IReviewRepository _reviewRepository;
        private readonly IAirlineRepository _airlineRepository;

        public ReviewService(IUserService userService, IReviewRepository reviewRepository, IAirlineRepository airlineRepository)
        {
            _userService = userService;
            _reviewRepository = reviewRepository;
            _airlineRepository = airlineRepository;
        }

        public async Task<AirlineReviewData> GetAirlineReviewsAsync(int airlineId)
        {
            var reviews = new List<Review>();
            var dalReviews = await _reviewRepository.GetAirlineReviews(airlineId);
            if (!dalReviews.Any())
            {
                var airline = await _airlineRepository.GetAirlineAsync(airlineId);

                return new AirlineReviewData
                {
                    AirlineId = airlineId,
                    AirlineName = airline.Name,
                    Reviews = new List<Review>()
                };
            }

            foreach (var dalReview in dalReviews)
            {
                var review = await ParseDalReviewAsync(dalReview);

                reviews.Add(review);
            }

            return new AirlineReviewData
            {
                AirlineId = airlineId,
                AirlineName = dalReviews.FirstOrDefault()?.Airline.Name,
                Reviews = reviews
            };
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