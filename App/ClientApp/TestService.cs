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
        Task<List<PersonDTO>?> GetPeopleAsync();
    }

    public class TestService : ITestService
    {
        private readonly HttpClient httpClient;
        string api_test = "https://localhost:7162/api/testhttp/";

        public TestService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<PersonDTO>?> GetPeopleAsync()
        {
            var url = "getPeople";
            return await httpClient.GetFromJsonAsync<List<PersonDTO>>(url);
        }
    }
}
