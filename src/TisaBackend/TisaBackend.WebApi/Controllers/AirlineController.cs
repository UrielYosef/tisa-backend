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

        [HttpGet]
        [Authorize(Roles = UserRoles.AdminAndAirlineManager)]
        [Route("{airlineId}/Agents")]
        public async Task<IActionResult> GetAirlineAgentsAsync([FromRoute] int airlineId)
        {
            return Ok(await _airlineService.GetAirlineAgentsAsync(airlineId));
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("{airlineId}/Airplanes")]
        public async Task<IActionResult> UpdateAirplanesAsync([FromBody] IList<AirplaneData> airplanesData, int airlineId)
        {
            await _airlineService.UpdateAirplanesAsync(airlineId, airplanesData);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("New")]
        public async Task<IActionResult> AddAirlineAsync([FromBody] NewAirlineRequest newAirlineRequest)
        {
            var isSuccess = await _airlineService.TryAddAirlineAsync(newAirlineRequest);

            return Ok(isSuccess);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.AdminAndAirlineManager)]
        [Route("NewAgent")]
        public async Task<IActionResult> AddAirlineAgentAsync([FromBody] NewAirlineAgentRequest newAirlineAgentRequest)
        {
            var result = await _airlineService.TryAddAirlineAgentAsync(newAirlineAgentRequest);
            
            return Ok(result);
        }
    }
}