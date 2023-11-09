using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp
{
    public static class TestHttpClient
    {
        static string api_test = "https://localhost:7162/api/testhttp/";
        static string api_user = "https://localhost:7162/api/user/";

        /// <summary>
        /// realiza llamada post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task PersonCreateAsync(JsonSerializerOptions options)
        {
            var url = $"{api_test}createPerson";

            PersonDTO personDTO = new PersonDTO()
            {
                FirstName = "Oliver",
                LastName = "Hurtado",
            };

            {
                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(url, personDTO);

                await Console.Out.WriteLineAsync($"{response.IsSuccessStatusCode}");
                var str = await response.Content.ReadAsStringAsync();
                await Console.Out.WriteLineAsync(str);
            }
            {
                using var httpClient = new HttpClient();
                var str = JsonSerializer.Serialize(personDTO, options);
                var content = new StringContent(str, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                await Console.Out.WriteLineAsync($"{response.IsSuccessStatusCode}");
                str = await response.Content.ReadAsStringAsync();
                await Console.Out.WriteLineAsync(str);
            }
        }

        /// <summary>
        /// realiza llamada get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task PersonReadAsync(JsonSerializerOptions options)
        {
            var url = $"{api_test}getPeople";

            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);
                var str = await response.Content.ReadAsStringAsync();
                var people = JsonSerializer.Deserialize<List<PersonDTO>>(str, options);
            }
            {
                using var httpClient = new HttpClient();
                var str = await httpClient.GetStringAsync(url);
                var people = JsonSerializer.Deserialize<List<PersonDTO>>(str, options);
            }
        }

        public static async Task PersonUpdateAsync(JsonSerializerOptions options)
        {
            var url = $"{api_test}updatePerson";

            PersonDTO personDTO = new PersonDTO()
            {
                FirstName = "Nahara",
                LastName = "Brizuela",
            };

            using var httpClient = new HttpClient();
            var response = await httpClient.PutAsJsonAsync($"{url}/1", personDTO);
        }

        public static async Task PersonDeleteAsync(JsonSerializerOptions options)
        {
            var url = $"{api_test}deletePerson";
            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync($"{url}/1");
        }

        /// <summary>
        /// agrega a valores a cabecera y hace get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task DummyAsync(JsonSerializerOptions options)
        {
            var url = $"{api_test}getDummy";

            {
                // header solo afecta primera peticion

                using var httpClient = new HttpClient();
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                requestMessage.Headers.Add("xxx", "10");
                var response = await httpClient.SendAsync(requestMessage);
                var str = await response.Content.ReadAsStringAsync();
                var dummy = JsonSerializer.Deserialize<DummyDTO>(str, options);

                var response2 = await httpClient.GetFromJsonAsync<DummyDTO>(url, options);
            }

            {
                // header afecta todas las peticiones

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("xxx", "20");
                var response2 = await httpClient.GetFromJsonAsync<DummyDTO>(url, options);
                var response3 = await httpClient.GetFromJsonAsync<DummyDTO>(url, options);
            }
        }

        /// <summary>
        /// agrega token jwt a cabecera y hace get
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task DummySafeAsync(JsonSerializerOptions options)
        {
            var url_signup = $"{api_user}signup";
            var url_login = $"{api_user}login";
            var url_safe = $"{api_test}getDummySafe";
            string? token = null;

            var signup = true;
            if (signup)
            {
                var dto = new UserSignUpEditorDTO()
                {
                    Email = "xxx@xxx.xxx",
                    FirstName = "111 111",
                    LastName = "222 222",
                    Password = "xxxxxx",
                };

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(url_signup, dto);

                await Console.Out.WriteLineAsync($"{response.IsSuccessStatusCode}");
                var str = await response.Content.ReadAsStringAsync();
                await Console.Out.WriteLineAsync(str);
            }

            var login = true;
            if (login)
            {
                //login

                var dto = new UserLogInEditorDTO()
                {
                    Email = "xxx@xxx.xxx",
                    Password = "xxxxxx",
                };

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(url_login, dto);

                await Console.Out.WriteLineAsync($"{response.IsSuccessStatusCode}");
                var str = await response.Content.ReadAsStringAsync();
                await Console.Out.WriteLineAsync(str);

                var userToken = JsonSerializer.Deserialize<UserTokenDTO>(str, options);
                token = userToken?.Token;
            }

            var dummySafe = true;
            if (dummySafe)
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var response2 = await httpClient.GetFromJsonAsync<DummyDTO>(url_safe, options);
            }
        }
    }
}
