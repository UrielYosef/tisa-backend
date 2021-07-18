using System;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TisaBackend.Domain;
using TisaBackend.Domain.Auth;
using TisaBackend.Domain.Interfaces.BL;

namespace TisaBackend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthenticateController(IUserService userService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
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
        [Authorize(Roles = UserRoles.AdminAndAirlineManager)]
        [Route("RegisterAirlineAgent")]
        public async Task<IActionResult> RegisterAirlineAgentAsync([FromBody] SignUpModel signUpModel)
        {
            var userExists = await _userManager.FindByNameAsync(signUpModel.Username)
                             ?? await _userManager.FindByEmailAsync(signUpModel.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User already exists" });

            var user = new User
            {
                Email = signUpModel.Email,
                UserName = signUpModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User creation failed" });

            if (!await _roleManager.RoleExistsAsync(UserRoles.AirlineAgent))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.AirlineAgent));
            await _userManager.AddToRoleAsync(user, UserRoles.AirlineAgent);

            return Ok(new { Status = "Success", Message = "User created successfully" });
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("RegisterAirlineManager")]
        public async Task<IActionResult> RegisterAirlineManagerAsync([FromBody] SignUpModel signUpModel)
        {
            var userExists = await _userManager.FindByNameAsync(signUpModel.Username)
                             ?? await _userManager.FindByEmailAsync(signUpModel.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User already exists" });

            var user = new User
            {
                Email = signUpModel.Email,
                UserName = signUpModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User creation failed" });

            if (!await _roleManager.RoleExistsAsync(UserRoles.AirlineManager))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.AirlineManager));
            await _userManager.AddToRoleAsync(user, UserRoles.AirlineManager);

            return Ok(new { Status = "Success", Message = "User created successfully" });
        }

        [HttpPost]
        //[Authorize(Roles = UserRoles.Admin)]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] SignUpModel signUpModel)
        {
            var userExists = await _userManager.FindByNameAsync(signUpModel.Username)
                             ?? await _userManager.FindByEmailAsync(signUpModel.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists" });

            var user = new User
            {
                Email = signUpModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = signUpModel.Username,
            };
            var result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed" });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(new { Status = "Success", Message = "User created successfully" });
        }
    }
}