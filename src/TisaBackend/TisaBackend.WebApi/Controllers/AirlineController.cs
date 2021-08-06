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
        [Authorize(Roles = UserRoles.Admin)]
        [Route("All")]
        public async Task<IActionResult> GetAllAirlinesAsync()
        {
            var airlines = await _airlineService.GetAllAirlinesAsync();

            return Ok(airlines);
        }
        
        [HttpGet]
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("{airlineId}/Airplanes")]
        public async Task<IActionResult> GetAirlineAirplanesAsync([FromRoute] int airlineId)
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);
            var airplanes = await _airlineService.GetAirlineAirplanesAsync(airlineId, User.Identity.Name, isAdmin);

            return Ok(airplanes);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.AdminAndAirlineManager)]
        [Route("{airlineId}/Agents")]
        public async Task<IActionResult> GetAirlineAgentsAsync([FromRoute] int airlineId)
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);
            var agents = await _airlineService.GetAirlineAgentsAsync(airlineId, User.Identity.Name, isAdmin);

            return Ok(agents);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("{airlineId}/Airplanes")]
        public async Task<IActionResult> UpdateAirplanesAsync([FromBody] IList<AirplaneData> airplanesData, int airlineId)
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);
            await _airlineService.UpdateAirplanesAsync(airlineId, airplanesData, User.Identity.Name, isAdmin);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddAirlineAsync([FromBody] NewAirlineRequest newAirlineRequest)
        {
            var isSuccess = await _airlineService.TryAddAirlineAsync(newAirlineRequest);

            return Ok(isSuccess);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.AdminAndAirlineManager)]
        [Route("Agent")]
        public async Task<IActionResult> AddAirlineAgentAsync([FromBody] NewAirlineAgentRequest newAirlineAgentRequest)
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);
            var result = await _airlineService.TryAddAirlineAgentAsync(newAirlineAgentRequest, User.Identity.Name, isAdmin);
            
            return Ok(result);
        }
    }
}