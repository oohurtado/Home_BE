using Home.Source.Services.Github;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            this.gitHubService = gitHubService;
        }

        [HttpGet(template: "getProfile")]
        public async Task<ActionResult<string>> GetProfile(string user)
        {
            try
            {
                var result = await gitHubService.GetProfileAsync(user);
                return result;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
