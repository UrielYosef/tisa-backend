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
            await _flightService.AddNewFlightAsync(airlineId, newFlight);

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("Airline/{airlineId}")]
        public async Task<IActionResult> GetFlightsAsync(int airlineId)
        {
            var airlineFlights = await _flightService.GetFlightsInANutshellAsync(airlineId);

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
    }
}