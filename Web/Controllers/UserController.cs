using Home.Source.BusinessLayer;
using Home.Source.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserLayer userLayer;
        private readonly ConfigurationLayer configurationLayer;

        public UserController(
            UserLayer userLayer,
            ConfigurationLayer configurationLayer)
        {
            this.userLayer = userLayer;
            this.configurationLayer = configurationLayer;
        }

        [HttpPost(template: "signup")]
        public async Task<ActionResult<UserTokenDTO>> SignUp([FromBody] UserSignUpEditorDTO dto)
        {
            var result = await userLayer.SignUpAsync(dto);
            if (result.Succeeded)
            {
                var user = await userLayer.FindByEmailAsync(dto.Email);
                var isAdmin = userLayer.IsEmailAdmin(dto.Email);
                var roles = GetInitRole(isAdmin);
                await userLayer.AddRolesToUserAsync(user, roles);
                var claims = TokenHelper.CreateClaims(user!, roles);
                var token = TokenHelper.BuildToken(claims, configurationLayer.GetJWTKey());
                return token;
            }

            return BadRequest(result?.Errors.Select(p => p.Description).ToList());
        }

        [HttpPost(template: "login")]
        public async Task<ActionResult<UserTokenDTO>> LogIn([FromBody] UserLogInEditorDTO dto)
        {
            var result = await userLayer.LogInAsync(dto.Email, dto.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await userLayer.FindByEmailAsync(dto.Email);
                var roles = await userLayer.GetUserRolesAsync(user);
                var claims = TokenHelper.CreateClaims(user!, roles.ToList());
                var token = TokenHelper.BuildToken(claims, configurationLayer.GetJWTKey());

                return token;
            }

            return BadRequest(new List<string>() { "Error, wrong credentials, try again." });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(template: "changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordEditorDTO dto)
        {
            var user = await userLayer.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email)!);
            if (user == null)
            {
                return BadRequest(new List<string>() { "Error, user not found." });
            }

            var result = await userLayer.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result?.Errors.Select(p => p.Description).ToList());
            }
        }

        [HttpGet(template: "isEmailAvailable/{email}")]
        public async Task<ActionResult> IsEmailAvailable(string email)
        {
            var user = await userLayer.FindByEmailAsync(email);

            bool isAvailable = user == null;
            return Ok(isAvailable);
        }

        private static List<string> GetInitRole(bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<string> { "admin" };
            }

            return new List<string> { "user" };
        }
    }
}
