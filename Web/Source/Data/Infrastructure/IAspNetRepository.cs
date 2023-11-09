using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Home.Source.Data.Infrastructure
{
    public interface IAspNetRepository
    {
        Task AddRoleToUserAsync(User? user, string role);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task CreateRolesAsync();
        Task DeleteRolesAsync();
        Task<User?> FindByEmailAsync(string email);
        Task<IList<string>> GetUserRolesAsync(User? user);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);
        Task<IList<User>> GetUsersAsync(string? who);
    }
}