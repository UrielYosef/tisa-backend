using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TisaBackend.Domain;
using TisaBackend.Domain.Models.Auth;
using TisaBackend.Domain.Interfaces.BL;

namespace TisaBackend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticateController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInModel signInModel)
        {
            var signInResult = await _userService.SignInAsync(signInModel);

            return StatusCode(signInResult.StatusCode, signInResult.SignInDetails);
        }

        //TODO: how to check if airline manager is the manager of the current airline request?
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpModel signUpModel)
        {
            var signUpResult = await _userService.SignUpAsync(signUpModel);

            return StatusCode(signUpResult.StatusCode, signUpResult);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] SignUpModel signUpModel)
        {
            var signUpResult = await _userService.RegisterAdminAsync(signUpModel);

            return StatusCode(signUpResult.StatusCode, signUpResult);
        }
    }
}