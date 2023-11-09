using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public interface ITestService
    {
        Task<List<TestDTO>?> GetDataAsync();
    }

    public class TestService : ITestService
    {
        private readonly HttpClient httpClient;
        //string api_test = "https://localhost:7162/api/testhttp/";

        public TestService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<TestDTO>?> GetDataAsync()
        {
            var url = "getData";
            return await httpClient.GetFromJsonAsync<List<TestDTO>>(url);
        }
    }
}
