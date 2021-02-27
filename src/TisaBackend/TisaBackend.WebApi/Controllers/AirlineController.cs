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
    public class AirlineController : ControllerBase
    {
        private readonly IAirlineService _airlineService;

        public AirlineController(IAirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> AddAirlineAsync([FromBody] Airline airline)
        {
            await _airlineService.AddAirlineAsync(airline);

            return Ok();
        }

        [Authorize(Roles = UserRoles.AirlineAgent)]
        [Route("Airplane")]
        [HttpPut]
        public async Task<IActionResult> AddAirplaneAsync([FromBody] Airplane airplane)
        {
            await _airlineService.AddAirplaneAsync(airplane);

            return Ok();
        }
    }
}