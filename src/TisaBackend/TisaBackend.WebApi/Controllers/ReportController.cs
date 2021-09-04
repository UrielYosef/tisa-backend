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
        [Route("Admin")]
        public async Task<IActionResult> GetReportAsync()
        {
            var isAdmin = User.IsInRole(UserRoles.Admin);

            var reports = await _reportService.GetAirlinesReportsDataAsync(User.Identity.Name, isAdmin);

            return Ok(reports);
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
    }
}