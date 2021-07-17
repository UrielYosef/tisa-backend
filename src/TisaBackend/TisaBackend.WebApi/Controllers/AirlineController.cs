using System.Threading.Tasks;
using System.Collections.Generic;
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
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("{airlineId}/Airplanes")]
        public async Task<IActionResult> GetAirlineAirplanesAsync([FromRoute] int airlineId)
        {
            return Ok(await _airlineService.GetAirlineAirplanesAsync(airlineId));
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddAirlineAsync([FromBody] NewAirlineRequest newAirlineRequest)
        {
            await _airlineService.AddAirlineAsync(newAirlineRequest);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("{airlineId}/Airplanes")]
        public async Task<IActionResult> UpdateAirplanesAsync([FromBody] IList<AirplaneData> airplanesData, int airlineId)
        {
            await _airlineService.UpdateAirplanesAsync(airlineId, airplanesData);

            return Ok();
        }
    }
}