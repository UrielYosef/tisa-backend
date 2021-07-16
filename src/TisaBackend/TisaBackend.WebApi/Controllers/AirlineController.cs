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
    public class AirlineController : ControllerBase
    {
        private readonly IAirlineService _airlineService;

        public AirlineController(IAirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAllAirlinesAsync()
        {
            return Ok(await _airlineService.GetAllAirlinesAsync());
        }

        [HttpGet]
        [Route("{airlineId}")]
        public async Task<IActionResult> GetAirlineAirplanesAsync([FromRoute] int airlineId)
        {
            return Ok(await _airlineService.GetAirlineAirplanesAsync(airlineId));
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddAirlineAsync([FromBody] Airline airline)
        {
            await _airlineService.AddAirlineAsync(airline);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.AirlineManager + "," + UserRoles.AirlineAgent)]
        [Route("Airplane")]
        public async Task<IActionResult> AddAirplaneAsync([FromBody] Airplane airplane)
        {
            await _airlineService.AddAirplaneAsync(airplane);

            return Ok();
        }
    }
}