﻿using Microsoft.Extensions.DependencyInjection;
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
        //static string api_user = "https://localhost:7162/api/user/";
        static string api_weather = "https://localhost:7162/weatherforecast/";

        static ITestService? _testService;

        public static IHttpClientFactory GetHttpClientFactory(bool named = false, Action extra = null!)
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
                    options.BaseAddress = new Uri("https://localhost:7162/api/testhttp/getData");
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

                serviceCollection.AddHttpClient<ITestService, TestService>("interface", options =>
                {
                    options.BaseAddress = new Uri("https://localhost:7162/api/testhttp/");
                    options.DefaultRequestHeaders.Add("xxx", "100");
                });
            }    


            var services = serviceCollection.BuildServiceProvider();
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

            _testService = services.GetRequiredService<ITestService>();

            return httpClientFactory;
        }

        public static async Task DataReadAsync(JsonSerializerOptions options)
        {
            var httpClientFactory = GetHttpClientFactory();
            var httpClient = httpClientFactory.CreateClient();

            var url = $"{api_test}getData";
            var response = await httpClient.GetAsync(url);
            var str = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<TestDTO>>(str, options);

            await Task.Delay(1);
        }

        public async static Task DataReadNamedAsync(JsonSerializerOptions options)
        {
            var httpClientFactory = GetHttpClientFactory(named: true);
            var httpClient_1 = httpClientFactory.CreateClient("test-http-1");
            var httpClient_2 = httpClientFactory.CreateClient("test-http-2");

            { 
                var response = await httpClient_1.GetAsync("");
                var str = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<TestDTO>>(str, options);
            }

            {
                var url = $"getData";
                var response = await httpClient_2.GetAsync(url);
                var str = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<TestDTO>>(str, options);
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

        public static async Task DataReadServiceAsync(JsonSerializerOptions options)
        {
            var httpClientFactory = GetHttpClientFactory(named: true);
            var httpClient = httpClientFactory.CreateClient("interface");
            var data = await _testService?.GetDataAsync()!;
        }
    }
}





