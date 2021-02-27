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
    public class AirplaneController : ControllerBase
    {
        private readonly IAirplaneTypeService _airplaneTypeService;

        public AirplaneController(IAirplaneTypeService airplaneTypeService)
        {
            _airplaneTypeService = airplaneTypeService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [Route("Type")]
        [HttpGet]
        public async Task<IActionResult> GetAirplaneTypesAsync()
        {
            var airplaneTypes = await _airplaneTypeService.GetAirplaneTypesAsync();

            return Ok(airplaneTypes);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [Route("Type")]
        [HttpPut]
        public async Task<IActionResult> AddAirplaneTypesAsync([FromBody] AirplaneType airplaneType)
        {
            await _airplaneTypeService.AddAirplaneTypeAsync(airplaneType);

            return Ok();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [Route("Department")]
        [HttpPut]
        public async Task<IActionResult> AddAirplaneDepartmentTypesAsync([FromBody] AirplaneDepartmentType airplaneDepartmentType)
        {
            await _airplaneTypeService.AddAirplaneDepartmentTypeAsync(airplaneDepartmentType);

            return Ok();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [Route("DepartmentSeats")]
        [HttpPut]
        public async Task<IActionResult> AddAirplaneDepartmentSeatsAsync([FromBody] AirplaneDepartmentSeats airplaneDepartmentSeats)
        {
            await _airplaneTypeService.AddAirplaneDepartmentSeatsAsync(airplaneDepartmentSeats);

            return Ok();
        }
    }
}