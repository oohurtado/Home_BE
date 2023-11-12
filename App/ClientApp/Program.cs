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
                await TestHttpClient.DataCreateAsync(options);
                return;
            }

            if (input == "2")
            {
                await TestHttpClient.DataReadAsync(options);
                return;
            }

            if (input == "3")
            {
                await TestHttpClient.DataUpdateAsync(options);
                return;
            }

            if (input == "4")
            {
                await TestHttpClient.DataDeleteAsync(options);
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
                await TestHttpClientFactory.DataReadAsync(options);
                return;
            }

            if (input == "8")
            {
                await TestHttpClientFactory.DataReadNamedAsync(options);
                return;
            }

            if (input == "9")
            {
                await TestHttpClientFactory.DummyAsync(options);
                return;
            }

            if (input == "10")
            {
                await TestHttpClientFactory.DataReadServiceAsync(options);
                return;
            }

            if (input == "11")
            {
                List<string> cards = await TestTask.GetCadsAsync(nCards: 5);
                var cardsResult = await TestTask.ProcessCardsAsync(cards);
            }
        }
    }
}