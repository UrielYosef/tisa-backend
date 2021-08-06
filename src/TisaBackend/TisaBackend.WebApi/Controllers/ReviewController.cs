using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TisaBackend.Domain;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Models;

namespace TisaBackend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [Route("Airline/{airlineId}")]
        public async Task<IActionResult> GetAirlineReviewsAsync(int airlineId)
        {
            var reviews = await _reviewService.GetAirlineReviewsAsync(airlineId);

            return Ok(reviews);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.AllRoles)]
        public async Task<IActionResult> AddReviewAsync([FromBody] Review review)
        {
            review.Username = User.Identity.Name;
            await _reviewService.AddReviewAsync(review);

            return Ok();
        }
    }
}