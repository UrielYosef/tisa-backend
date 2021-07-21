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
    public class AirportController : ControllerBase
    {
        private readonly IAirportService _airportService;

        public AirportController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAirportsAsync(string filter)
        {
            var airports = await _airportService.GetAirportsAsync(filter);

            return Ok(airports);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> AddAirportAsync([FromBody] Airport airport)
        {
            await _airportService.AddAirportAsync(airport);

            return Ok();
        }
    }
}