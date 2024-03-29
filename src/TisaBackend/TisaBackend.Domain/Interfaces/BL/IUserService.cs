﻿using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models.Auth;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IUserService
    {
        Task<SignUpResult> RegisterAdminAsync(SignUpModel signUpModel);
        Task<SignInResult> SignInAsync(SignInModel signInModel);
        Task<SignUpResult> SignUpAsync(SignUpModel signUpModel);
        Task<User> FindUserByEmailAsync(string email);
        Task<User> FindUserByUsernameAsync(string username);
        Task<User> FindUserByUserIdAsync(string userId);
        Task<string> GetUserIdByUsernameAsync(string username);
        Task<User> CreateNewUserAsync(string email, string role);
        Task AddRoleToUserAsync(string userEmail, string role);
        Task<bool> TryAddUserToAirlineAsync(string userId, int airlineId);
        Task<IList<string>> GetUsersEmailsAsync(int airlineId);
        Task<bool> IsAuthorizeForAirlineAsync(int airlineId, string username, bool isAdmin);
        Task<bool> IsAirlineManagerAsync(int airlineId, string username, bool isAdmin);
        Task<IList<(User user, string roles)>> GetRegisteredUsersAsync();
    }
}