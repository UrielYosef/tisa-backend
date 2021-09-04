using System;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TisaBackend.DAL;
using TisaBackend.Domain;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Models;
using TisaBackend.Domain.Models.Auth;
using SignInResult = TisaBackend.Domain.Models.Auth.SignInResult;

namespace TisaBackend.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly string _jwtSecret;
        private readonly string _jwtValidIssuer;
        private readonly string _jwtValidAudience;
        private readonly int _jwtExpirationInHours;
        private readonly int _userCreationByEmailAttempts;

        public UserService(IConfiguration config, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            _jwtSecret = config.GetSection("JWT").GetSection("Secret").Get<string>();
            _jwtValidIssuer = config.GetSection("JWT").GetSection("ValidIssuer").Get<string>();
            _jwtValidAudience = config.GetSection("JWT").GetSection("ValidAudience").Get<string>();
            _jwtExpirationInHours = config.GetSection("JWT").GetSection("ExpirationInHours").Get<int>();

            _userCreationByEmailAttempts = config
                .GetSection("UserService").GetSection("userCreationByEmailAttempts").Get<int>();
        }

        public async Task<SignUpResult> RegisterAdminAsync(SignUpModel signUpModel)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userExists = await userManager.FindByNameAsync(signUpModel.Username)
                             ?? await userManager.FindByEmailAsync(signUpModel.Email);
            if (userExists != null)
                return new SignUpResult(StatusCodes.Status400BadRequest,
                    "Error", "User already exists");

            var user = new User
            {
                Email = signUpModel.Email,
                UserName = signUpModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, signUpModel.Password);
            if (!result.Succeeded)
                return new SignUpResult(StatusCodes.Status500InternalServerError,
                    "Error", "User creation failed");

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            await userManager.AddToRoleAsync(user, UserRoles.Admin);

            return new SignUpResult(StatusCodes.Status200OK,
                "Success", "User created successfully");
        }

        public async Task<SignInResult> SignInAsync(SignInModel signInModel)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var user = await userManager.FindByNameAsync(signInModel.Username);
            if (user is null || !await userManager.CheckPasswordAsync(user, signInModel.Password))
                return new SignInResult(StatusCodes.Status404NotFound);

            var userRoles = await userManager.GetRolesAsync(user);

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
                var airline = await GetAirlineAsync(user.Id);
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
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userExists = await userManager.FindByNameAsync(signUpModel.Username)
                             ?? await userManager.FindByEmailAsync(signUpModel.Email);
            if (userExists != null)
                return new SignUpResult(StatusCodes.Status400BadRequest, 
                    "Error",  "User already exists");

            var user = new User
            {
                Email = signUpModel.Email,
                UserName = signUpModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, signUpModel.Password);
            if (!result.Succeeded)
                return new SignUpResult(StatusCodes.Status500InternalServerError,
                    "Error", "User creation failed");

            if (!await roleManager.RoleExistsAsync(UserRoles.Client))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Client));
            await userManager.AddToRoleAsync(user, UserRoles.Client);

            return new SignUpResult(StatusCodes.Status200OK,
                "Success", "User created successfully");
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            return await userManager.FindByEmailAsync(email);
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            return await userManager.FindByNameAsync(username);
        }

        public async Task<User> FindUserByUserIdAsync(string userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            return await userManager.FindByIdAsync(userId);
        }

        public async Task<string> GetUserIdByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var user = await userManager.FindByNameAsync(username);

            return user?.Id;
        }

        public async Task<User> CreateNewUserAsync(string email, string role)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var user = new User
            {
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var username = user.Email.Substring(0, user.Email.IndexOf('@'));
            if (await userManager.FindByNameAsync(username) != null)
            {
                int attempts = 1;
                while (attempts <= _userCreationByEmailAttempts)
                {
                    var userExists = await userManager.FindByNameAsync(username + attempts);
                    if (userExists is null)
                    {
                        username += attempts;
                        break;
                    }
                    attempts++;
                }
            }

            user.UserName = username;
            var password = "XYZxyz123"; //Just for Development
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return null;

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            await userManager.AddToRoleAsync(user, role);

            return user;
        }

        public async Task AddRoleToUserAsync(string userEmail, string role)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var user = await userManager.FindByEmailAsync(userEmail);
            if (user is null)
                return;

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            await userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> TryAddUserToAirlineAsync(string userId, int airlineId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var tisaContext = scope.ServiceProvider.GetRequiredService<TisaContext>();

            var existingMap = tisaContext.UserToAirline
                .FirstOrDefault(userToAirline => userToAirline.UserId.Equals(userId));
            if (existingMap != null)
                return false;

            await tisaContext.UserToAirline.AddAsync(new UserToAirline(userId, airlineId));
            await tisaContext.SaveChangesAsync();

            return true;
        }

        public async Task<IList<string>> GetUsersEmails(int airlineId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var tisaContext = scope.ServiceProvider.GetRequiredService<TisaContext>();

            var airlineUsersEmail = await tisaContext.UserToAirline
                .Include(userToAirline => userToAirline.User)
                .Where(userToAirline => userToAirline.AirlineId.Equals(airlineId))
                .Select(userToAirline => userToAirline.User.Email)
                .ToListAsync();

            return airlineUsersEmail;
        }

        public async Task<bool> IsAuthorizeForAirlineAsync(int airlineId, string username, bool isAdmin)
        {
            if (isAdmin)
                return true;

            var user = await FindUserByUsernameAsync(username);
            var userEmail = user?.Email;
            var airlineEmails = await GetUsersEmails(airlineId);

            return !string.IsNullOrEmpty(userEmail) && airlineEmails.Contains(userEmail);
        }

        public async Task<bool> IsAirlineManager(int airlineId, string username, bool isAdmin)
        {
            if (isAdmin)
                return true;

            var user = await FindUserByUsernameAsync(username);
            if (user is null)
                return false;

            var userEmail = user.Email;
            var airline = await GetAirlineAsync(user.Id);

            return airline.Id.Equals(airlineId) && userEmail.Equals(airline.AirlineManagerEmail);
        }

        private async Task<Airline> GetAirlineAsync(string userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var tisaContext = scope.ServiceProvider.GetRequiredService<TisaContext>();

            return await tisaContext.UserToAirline
                .Include(userToAirline => userToAirline.Airline)
                .Where(userToAirline => userToAirline.UserId.Equals(userId))
                .Select(userToAirline => userToAirline.Airline)
                .SingleOrDefaultAsync();
        }
    }
}