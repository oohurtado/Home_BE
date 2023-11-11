namespace Home.Source.Services.Github
{
    public interface IGitHubService
    {
        Task<string> GetProfileAsync(string user);
    }
}