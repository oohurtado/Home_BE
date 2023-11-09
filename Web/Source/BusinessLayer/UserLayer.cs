using Home.Source.Data.Infrastructure;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs;

namespace Home.Source.BusinessLayer
{
    public class UserLayer
    {
        private readonly IAspNetRepository aspNetRepository;
        private readonly ConfigurationLayer configurationLayer;

        public UserLayer(
            IAspNetRepository aspNetRepository, 
            ConfigurationLayer configurationLayer
            )
        {
            this.aspNetRepository = aspNetRepository;
            this.configurationLayer = configurationLayer;
        }

        public async Task<IdentityResult> SignUpAsync(UserSignUpEditorDTO dto)
        {
            User user = new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };

            return await aspNetRepository.CreateAsync(user, dto.Password);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await aspNetRepository.FindByEmailAsync(email);
        }

        public bool IsEmailAdmin(string email)
        {
            return configurationLayer.IsEmailAdmin(email);
        }

        public async Task AddRolesToUserAsync(User? user, List<string> roles)
        {
            foreach (var role in roles)
            {
                await aspNetRepository.AddRoleToUserAsync(user!, role);
            }
        }

        public async Task<SignInResult> LogInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await aspNetRepository.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);
        }

        public async Task<IList<string>> GetUserRolesAsync(User? user)
        {
            return await aspNetRepository.GetUserRolesAsync(user!);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await aspNetRepository.ChangePasswordAsync(user, currentPassword, newPassword);
        }
    }
}
