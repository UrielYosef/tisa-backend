using System;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TisaBackend.Domain;
using TisaBackend.Domain.Auth;
using TisaBackend.Domain.Interfaces.BL;
using SignInResult = TisaBackend.Domain.SignInResult;

namespace TisaBackend.BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAirlineService _airlineService;

        private readonly Random _random;
        private readonly string _jwtSecret;
        private readonly string _jwtValidIssuer;
        private readonly string _jwtValidAudience;
        private readonly int _jwtExpirationInHours;
        private readonly string _lowerCaseCharacters;
        private readonly string _upperCaseCharacters;
        private readonly string _digitsCharacters;
        private readonly int _userCreationByEmailAttempts;

        public UserService(IConfiguration config, IAirlineService airlineService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _airlineService = airlineService;

            _jwtSecret = config.GetSection("JWT").GetSection("Secret").Get<string>();
            _jwtValidIssuer = config.GetSection("JWT").GetSection("ValidIssuer").Get<string>();
            _jwtValidAudience = config.GetSection("JWT").GetSection("ValidAudience").Get<string>();
            _jwtExpirationInHours = config.GetSection("JWT").GetSection("ExpirationInHours").Get<int>();

            _userCreationByEmailAttempts = config
                .GetSection("UserService").GetSection("userCreationByEmailAttempts").Get<int>();
            _lowerCaseCharacters = config
                .GetSection("UserService").GetSection("lowerCaseCharacters").Get<string>();
            _upperCaseCharacters = config
                .GetSection("UserService").GetSection("upperCaseCharacters").Get<string>();
            _digitsCharacters = config
                .GetSection("UserService").GetSection("digitsCharacters").Get<string>();

            _random = new Random();
        }

        public async Task<SignInResult> SignInAsync(SignInModel signInModel)
        {
            var user = await _userManager.FindByNameAsync(signInModel.Username);
            if (user is null || !await _userManager.CheckPasswordAsync(user, signInModel.Password))
                return new SignInResult(StatusCodes.Status404NotFound);

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
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));

            var token = new JwtSecurityToken(
                issuer: _jwtValidIssuer,
                audience: _jwtValidAudience,
                expires: DateTime.Now.AddHours(_jwtExpirationInHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            SignInDetails details;
            if (userRoles.Contains(UserRoles.AirlineManager) || userRoles.Contains(UserRoles.AirlineAgent))
            {
                var airline = await _airlineService.GetAirlineByUserIdAsync(user.Id);
                details = new AirlineSignInDetails
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Role = userRoles.FirstOrDefault(),
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    TokenExpiration = token.ValidTo,
                    AirlineId = airline.Id,
                    AirlineName = airline.Name,
                };
            }
            else
            {
                details = new SignInDetails
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Role = userRoles.FirstOrDefault(),
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    TokenExpiration = token.ValidTo
                };
            }

            return new SignInResult(StatusCodes.Status200OK, details);
        }

        public async Task<SignUpResult> SignUpAsync(SignUpModel signUpModel)
        {
            var userExists = await _userManager.FindByNameAsync(signUpModel.Username)
                             ?? await _userManager.FindByEmailAsync(signUpModel.Email);
            if (userExists != null)
                return new SignUpResult(StatusCodes.Status400BadRequest, 
                    "Error",  "User already exists");

            var user = new User
            {
                Email = signUpModel.Email,
                UserName = signUpModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, signUpModel.Password);
            if (!result.Succeeded)
                return new SignUpResult(StatusCodes.Status500InternalServerError,
                    "Error", "User creation failed");

            if (!await _roleManager.RoleExistsAsync(UserRoles.Client))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Client));
            await _userManager.AddToRoleAsync(user, UserRoles.Client);

            return new SignUpResult(StatusCodes.Status200OK,
                "Success", "User created successfully");
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> CreateNewUserAsync(string email)
        {
            var user = new User
            {
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var userName = user.Email.Substring(0, user.Email.IndexOf('@'));
            if (await _userManager.FindByNameAsync(userName) != null)
            {
                int attempts = 1;
                while (attempts <= _userCreationByEmailAttempts)
                {
                    var userExists = await _userManager.FindByNameAsync(userName + attempts);
                    if (userExists is null)
                    {
                        userName = userName + attempts;
                        break;
                    }
                    attempts++;
                }
            }

            user.UserName = userName;
            var password = GenerateNewPassword();
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return null;

            return user;
        }

        public async Task AddRoleAsync(User user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);
        }

        private string GenerateNewPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_lowerCaseCharacters[_random.Next(0, _lowerCaseCharacters.Length)]);
            builder.Append(_lowerCaseCharacters[_random.Next(0, _lowerCaseCharacters.Length)]);
            builder.Append(_lowerCaseCharacters[_random.Next(0, _lowerCaseCharacters.Length)]);

            builder.Append(_upperCaseCharacters[_random.Next(0, _upperCaseCharacters.Length)]);
            builder.Append(_upperCaseCharacters[_random.Next(0, _upperCaseCharacters.Length)]);
            builder.Append(_upperCaseCharacters[_random.Next(0, _upperCaseCharacters.Length)]);

            builder.Append(_digitsCharacters[_random.Next(0, _digitsCharacters.Length)]);
            builder.Append(_digitsCharacters[_random.Next(0, _digitsCharacters.Length)]);
            builder.Append(_digitsCharacters[_random.Next(0, _digitsCharacters.Length)]);

            var password = builder.ToString();
            var shuffledPassword = new string(password.ToCharArray().OrderBy(ch => Guid.NewGuid()).ToArray());

            return shuffledPassword;
        }
    }
}