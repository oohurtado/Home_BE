using Home.Source.BusinessLayer;
using Home.Source.Data.Infrastructure;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace Home.Source.Data.Repositories
{
    public class AspNetRepository : IAspNetRepository
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ConfigurationLayer configurationLayer;

        public AspNetRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            ConfigurationLayer configurationLayer
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.configurationLayer = configurationLayer;
        }

        /// <summary>
        /// Create roles from json field "Roles"
        /// </summary>
        public async Task CreateRolesAsync()
        {
            var databaseRoles = roleManager.Roles.ToList();
            var appRoles = configurationLayer.GetRoles();

            var roleNamesToAdd = appRoles.Except(databaseRoles.Select(p => p.Name));
            foreach (var role in roleNamesToAdd)
            {
                await roleManager.CreateAsync(new IdentityRole(role!));
            }
        }

        /// <summary>
        /// Deletes roles not in use and not in json field "Roles"
        /// </summary>
        public async Task DeleteRolesAsync()
        {
            var databaseRoles = await roleManager.Roles.ToListAsync();
            var appRoles = configurationLayer.GetRoles();

            var roleNamesToDelete = databaseRoles.Select(p => p.Name).Except(appRoles);
            var rolesToDelete = databaseRoles.Where(p => roleNamesToDelete.Contains(p.Name));
            foreach (var role in rolesToDelete)
            {
                await roleManager.DeleteAsync(role);
            }
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task AddRoleToUserAsync(User? user, string role)
        {
            await userManager.AddToRoleAsync(user!, role);
        }

        public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);
        }

        public async Task<IList<string>> GetUserRolesAsync(User? user)
        {
            return await userManager.GetRolesAsync(user!);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IList<User>> GetUsersAsync(string? who)
        {
            return await userManager.GetUsersInRoleAsync(who!);                 
        }
    }
}
