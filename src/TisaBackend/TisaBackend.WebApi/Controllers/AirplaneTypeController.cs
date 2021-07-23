using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TisaBackend.Domain;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Models;

namespace TisaBackend.WebApi.Controllers
{
    //TODO: remove comments at the end
    [Route("api/[controller]")]
    [ApiController]
    public class AirplaneTypeController : ControllerBase
    {
        private readonly IAirplaneTypeService _airplaneTypeService;

        public AirplaneTypeController(IAirplaneTypeService airplaneTypeService)
        {
            _airplaneTypeService = airplaneTypeService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.AdminAndAirlineManagerAndAirlineAgent)]
        [Route("{airplaneTypeId}/Departments")]
        public async Task<IActionResult> GetDepartmentTypesAsync(int airplaneTypeId)
        {
            var departmentTypes = await _airplaneTypeService.GetDepartmentTypesAsync(airplaneTypeId);

            return Ok(departmentTypes);
        }

        /*
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetAirplaneTypesAsync()
        {
            var airplaneTypes = await _airplaneTypeService.GetAirplaneTypesAsync();

            return Ok(airplaneTypes);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("AirplaneType")]
        public async Task<IActionResult> AddAirplaneTypeAsync([FromBody] AirplaneType airplaneType)
        {
            await _airplaneTypeService.AddAirplaneTypeAsync(airplaneType);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("DepartmentType")]
        public async Task<IActionResult> AddDepartmentTypeAsync([FromBody] DepartmentType departmentType)
        {
            await _airplaneTypeService.AddDepartmentTypeAsync(departmentType);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("AirplaneDepartmentSeats")]
        public async Task<IActionResult> AddSeatsToAirplaneTypeDepartmentAsync([FromBody] AirplaneDepartmentSeats airplaneDepartmentSeats)
        {
            await _airplaneTypeService.AddSeatsToAirplaneTypeDepartmentAsync(airplaneDepartmentSeats);

            return Ok();
        }*/
    }
}