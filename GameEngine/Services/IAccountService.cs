using GameEngine.Models;
using GameEngine.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GameEngine.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterUserViewModel model);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task SeedRolesAndAdminAsync();

        Task<ApplicationUser> GetById(string userId);
    }
}
