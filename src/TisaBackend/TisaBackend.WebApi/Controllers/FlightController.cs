using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TisaBackend.Domain;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Models;

namespace TisaBackend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [Authorize(Roles = UserRoles.AirlineAgent)]
        [HttpPut]
        public async Task<IActionResult> AddFlightAsync([FromBody] Flight flight)
        {
            await _flightService.AddFlightAsync(flight);

            return Ok();
        }
    }
}