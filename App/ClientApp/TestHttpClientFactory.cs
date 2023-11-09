using Microsoft.Extensions.DependencyInjection;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp
{
    public class TestHttpClientFactory
    {
        static string api_test = "https://localhost:7162/api/testhttp/";
        static string api_user = "https://localhost:7162/api/user/";
        static string api_weather = "https://localhost:7162/weatherforecast/";

        public static IHttpClientFactory GetHttpClientFactory(bool named = false)
        {
            var serviceCollection = new ServiceCollection();

            if (!named)
            {
                serviceCollection.AddHttpClient();
            }
            else
            {
                serviceCollection.AddHttpClient("test-http-1", options =>
                {
                    options.BaseAddress = new Uri("https://localhost:7162/api/testhttp/getPeople");
                });
                serviceCollection.AddHttpClient("test-http-2", options =>
                {
                    options.BaseAddress = new Uri("https://localhost:7162/api/testhttp/");
                    options.DefaultRequestHeaders.Add("xxx", "100");
                });

                serviceCollection.AddHttpClient("weather-forecast", options =>
                {
                    options.BaseAddress = new Uri(api_weather);
                });
            }
            
            var services = serviceCollection.BuildServiceProvider();
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
            return httpClientFactory;
        }

        public static async Task PersonReadAsync(JsonSerializerOptions options)
        {
            var httpClientFactory = GetHttpClientFactory();
            var httpClient = httpClientFactory.CreateClient();

            var url = $"{api_test}getPeople";
            var response = await httpClient.GetAsync(url);
            var str = await response.Content.ReadAsStringAsync();
            var people = JsonSerializer.Deserialize<List<PersonDTO>>(str, options);

            await Task.Delay(1);
        }

        public async static Task PersonReadNamedAsync(JsonSerializerOptions options)
        {
            var httpClientFactory = GetHttpClientFactory(named: true);
            var httpClient_1 = httpClientFactory.CreateClient("test-http-1");
            var httpClient_2 = httpClientFactory.CreateClient("test-http-2");

            { 
                var response = await httpClient_1.GetAsync("");
                var str = await response.Content.ReadAsStringAsync();
                var people = JsonSerializer.Deserialize<List<PersonDTO>>(str, options);
            }

            {
                var url = $"getPeople";
                var response = await httpClient_2.GetAsync(url);
                var str = await response.Content.ReadAsStringAsync();
                var people = JsonSerializer.Deserialize<List<PersonDTO>>(str, options);
            }

            await Task.Delay(1);
        }

        public static async Task DummyAsync(JsonSerializerOptions options)
        {
            var httpClientFactory = GetHttpClientFactory(named: true);
            var httpClient = httpClientFactory.CreateClient("test-http-2");

            var response = await httpClient.GetAsync("getDummy");
            var str = await response.Content.ReadAsStringAsync();
            var dummy = JsonSerializer.Deserialize<DummyDTO>(str, options);
        }
    }
}





