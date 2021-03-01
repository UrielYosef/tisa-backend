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
    public class AirplaneTypeController : ControllerBase
    {
        private readonly IAirplaneTypeService _airplaneTypeService;

        public AirplaneTypeController(IAirplaneTypeService airplaneTypeService)
        {
            _airplaneTypeService = airplaneTypeService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAirplaneTypesAsync()
        {
            var airplaneTypes = await _airplaneTypeService.GetAirplaneTypesAsync();

            return Ok(airplaneTypes);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> AddAirplaneTypesAsync([FromBody] AirplaneType airplaneType)
        {
            await _airplaneTypeService.AddAirplaneTypeAsync(airplaneType);

            return Ok();
        }
    }
}