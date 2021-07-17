using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TisaBackend.Domain;
using TisaBackend.Domain.Auth;
using TisaBackend.Domain.Interfaces;

namespace TisaBackend.BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly Random _random;
        private readonly int _userCreationByEmailAttempts;
        private readonly string _lowerCaseCharacters;
        private readonly string _upperCaseCharacters;
        private readonly string _digitsCharacters;

        public UserService(IConfiguration config, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

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

        public async Task ProvideAirlineManagerUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new User
                {
                    Email = email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                user = await CreateUserByEmailAsync(user);
                //TODO: send email to new user
            }

            await _userManager.RemoveFromRolesAsync(user, new List<string> { UserRoles.Client, UserRoles.AirlineAgent });
            if (!await _roleManager.RoleExistsAsync(UserRoles.AirlineManager))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.AirlineManager));
            await _userManager.AddToRoleAsync(user, UserRoles.AirlineManager);
        }

        private async Task<User> CreateUserByEmailAsync(User user)
        {
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