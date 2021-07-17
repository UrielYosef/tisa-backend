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

namespace TisaBackend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user is null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
                return Unauthorized();

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(4)).ToUnixTimeSeconds().ToString())
            };
            authClaims
                .AddRange(userRoles
                    .Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(4),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        //TODO: how to check if airline manager is the manager of the current airline request?
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationModel registrationModel)
        {
            var userExists = await _userManager.FindByNameAsync(registrationModel.Username) 
                             ?? await _userManager.FindByEmailAsync(registrationModel.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Status = "Error", Message = "User already exists" });

            var user = new User
            {
                Email = registrationModel.Email,
                UserName = registrationModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registrationModel.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User creation failed" });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Client))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Client));
            await _userManager.AddToRoleAsync(user, UserRoles.Client);

            return Ok(new { Status = "Success", Message = "User created successfully" });
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.AdminAndAirlineManager)]
        [Route("RegisterAirlineAgent")]
        public async Task<IActionResult> RegisterAirlineAgentAsync([FromBody] RegistrationModel registrationModel)
        {
            var userExists = await _userManager.FindByNameAsync(registrationModel.Username)
                             ?? await _userManager.FindByEmailAsync(registrationModel.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User already exists" });

            var user = new User
            {
                Email = registrationModel.Email,
                UserName = registrationModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, registrationModel.Password);

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
        public async Task<IActionResult> RegisterAirlineManagerAsync([FromBody] RegistrationModel registrationModel)
        {
            var userExists = await _userManager.FindByNameAsync(registrationModel.Username)
                             ?? await _userManager.FindByEmailAsync(registrationModel.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User already exists" });

            var user = new User
            {
                Email = registrationModel.Email,
                UserName = registrationModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, registrationModel.Password);

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
        public async Task<IActionResult> RegisterAdminAsync([FromBody] RegistrationModel registrationModel)
        {
            var userExists = await _userManager.FindByNameAsync(registrationModel.Username)
                             ?? await _userManager.FindByEmailAsync(registrationModel.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists" });

            var user = new User
            {
                Email = registrationModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registrationModel.Username,
            };
            var result = await _userManager.CreateAsync(user, registrationModel.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed" });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(new { Status = "Success", Message = "User created successfully" });
        }
    }
}