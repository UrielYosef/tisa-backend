using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TisaBackend.Domain;
using TisaBackend.Domain.Interfaces.BL;

namespace TisaBackend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Admin/Users")]
        public async Task<IActionResult> GetUsersReportAsync()
        {
            var report = await _reportService.GetRegisteredUsersAsync();

            return Ok(report);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Admin/Airlines")]
        public async Task<IActionResult> GetAirlinesReportAsync()
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);

            var report = await _reportService.GetAirlinesReportsDataAsync(User.Identity.Name, isAdmin);

            return Ok(report);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.AdminAndAirlineManager)]
        [Route("Airline/{airlineId}")]
        public async Task<IActionResult> GetReportAsync(int airlineId)
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);

            var report = await _reportService.GetAirlineReportDataAsync(airlineId, User.Identity.Name, isAdmin);

            return Ok(report);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.AdminAndAirlineManager)]
        [Route("Airline/Orders/{airlineId}")]
        public async Task<IActionResult> GetOrdersReportAsync(int airlineId)
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);

            var report = await _reportService.GetAirlineOrdersReportDataAsync(airlineId, User.Identity.Name, isAdmin);

            return Ok(report);
        }
    }
}