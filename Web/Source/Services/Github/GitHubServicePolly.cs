using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net.Http;

namespace Home.Source.Services.Github
{
    public class GitHubPollyService : IGitHubService
    {
        private readonly HttpClient httpClient;
        private readonly string api = "https://api.github.com/";
        private static readonly Random random = new Random();

        private readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy;

        public GitHubPollyService(HttpClient httpClient, ILogger<GitHubPollyService> logger)
        {
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", "request");

            //retryPolicy = Policy<HttpResponseMessage>
            //    .Handle<HttpRequestException>()
            //    .RetryAsync(retryCount: 3, onRetry: (ex, count, context) =>
            //    {
            //        logger.LogInformation($"*-*-*retry*-*-*, count: {count}");
            //    });

            //retryPolicy = Policy<HttpResponseMessage>
            //    .Handle<HttpRequestException>()   
            //    .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: tryAgain => TimeSpan.FromSeconds(3), onRetry: (ex, time, context) =>
            //    {
            //        logger.LogInformation($"*-*-*Retry*-*-*, time: {DateTime.Now}");
            //    });

            //retryPolicy = Policy<HttpResponseMessage>                
            //     .Handle<HttpRequestException>()
            //     .OrResult(p => 
            //        p.StatusCode == System.Net.HttpStatusCode.InternalServerError 
            //        || p.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
            //     .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: tryAgain => TimeSpan.FromSeconds(3), onRetry: (ex, time, context) =>
            //     {
            //         logger.LogInformation($"*-*-*Retry*-*-*, time: {DateTime.Now}");
            //     });

            retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: tryAgain => TimeSpan.FromSeconds(3), onRetry: (ex, time, context) =>
                {
                    logger.LogInformation($"*-*-*Retry*-*-*, time: {DateTime.Now}");
                });
        }


        public async Task<string> GetProfileAsync(string user)
        {
            var response = await retryPolicy.ExecuteAsync(async () =>
            {
                //if (random.Next(1, 3) == 1)
                //{
                //    throw new HttpRequestException();
                //}
                //return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

                var url = $"{api}users/{user}";
                return await httpClient.GetAsync(url);                
            });

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
