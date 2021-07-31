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
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("Airline/{airlineId}")]
        public async Task<IActionResult> AddFlightAsync(int airlineId, [FromBody] NewFlight newFlight)
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);
            await _flightService.AddNewFlightAsync(newFlight, airlineId, User.Identity.Name, isAdmin);

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("Airline/{airlineId}")]
        public async Task<IActionResult> GetFlightsAsync(int airlineId)
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);
            var airlineFlights = await _flightService.GetFlightsInANutshellAsync(airlineId, User.Identity.Name, isAdmin);

            return Ok(airlineFlights);
        }

        [HttpPost]
        [Route("Filter")]
        public async Task<IActionResult> FilterFlightsAsync([FromBody] FlightFilter flightFilter)
        {
            var filteredFlights = await _flightService.FilterFlightsAsync(flightFilter);

            return Ok(filteredFlights);
        }

        [HttpGet]
        [Route("{flightId}")]
        public async Task<IActionResult> GetFlightAsync(int flightId)
        {
            var flight = await _flightService.GetFullyDetailedFlightAsync(flightId);

            return Ok(flight);
        }

        [HttpPut]
        [Route("Order")]
        public async Task<IActionResult> AddFlightOrderAsync([FromBody] FlightOrder order)
        {
            await _flightService.AddFlightOrderAsync(order, User.Identity.Name);

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.AllRoles)]
        [Route("User/Future")]
        public async Task<IActionResult> GetUserIncomingFlightsAsync()
        {
            var flights = await _flightService.GetUserFlightsAsync(User.Identity.Name, true);

            return Ok(flights);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.AllRoles)]
        [Route("User/History")]
        public async Task<IActionResult> GetUserHistoryFlightsAsync()
        {
            var flights = await _flightService.GetUserFlightsAsync(User.Identity.Name, false);

            return Ok(flights);
        }
    }
}