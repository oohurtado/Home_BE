using Shared.DTOs;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ClientApp
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            while (true)
            {
                Console.Write("Option: ");
                var input = Console.ReadLine();

                await Console.Out.WriteLineAsync($"You selected: {input}");
                await HandleInputAsync(input);

                if (input == "x")
                    break;

                Console.Clear();
            }
        }

        private static async Task HandleInputAsync(string? input)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };



            if (input == "1")
            {
                await TestHttpClient.PersonCreateAsync(options);
                return;
            }

            if (input == "2")
            {
                await TestHttpClient.PersonReadAsync(options);
                return;
            }

            if (input == "3")
            {
                await TestHttpClient.PersonUpdateAsync(options);
                return;
            }

            if (input == "4")
            {
                await TestHttpClient.PersonDeleteAsync(options);
                return;
            }

            if (input == "5")
            {
                await TestHttpClient.DummyAsync(options);
                return;
            }

            if (input == "6")
            {
                await TestHttpClient.DummySafeAsync(options);
                return;
            }

            if (input == "7")
            {
                await TestHttpClientFactory.PersonReadAsync(options);
                return;
            }

            if (input == "8")
            {
                await TestHttpClientFactory.PersonReadNamedAsync(options);
                return;
            }

            if (input == "9")
            {
                await TestHttpClientFactory.DummyAsync(options);
                return;
            }

            if (input == "10")
            {
                await TestHttpClientFactory.PersonReadServiceAsync(options);
                return;
            }
        }
    }
}