using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TisaBackend.Domain;
using TisaBackend.Domain.Auth;
using TisaBackend.Domain.Interfaces;

namespace TisaBackend.BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> ProvideAirlineManagerUser(string email)
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

            return user.UserName;
        }

        private async Task<User> CreateUserByEmailAsync(User user)
        {
            var userName = user.Email.Substring(0, user.Email.IndexOf('@'));
            if (await _userManager.FindByNameAsync(userName) != null)
            {
                //TODO: config
                int attempts = 1;
                while (attempts <= 5)
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
            var result = await _userManager.CreateAsync(user, GenerateNewPassword());
            if (!result.Succeeded)
                return null;

            return user;
        }

        private string GenerateNewPassword()
        {
            //TODO: complete!
            return "Password123";
        }
    }
}