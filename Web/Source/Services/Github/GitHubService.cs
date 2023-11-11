using System.Net.Http;

namespace Home.Source.Services.Github
{
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient httpClient;
        private readonly string api = "https://api.github.com/";

        public GitHubService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
        }

        public async Task<string> GetProfileAsync(string user)
        {
            var url = $"{api}users/{user}";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
