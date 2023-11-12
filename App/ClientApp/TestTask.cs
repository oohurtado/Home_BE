using Newtonsoft.Json;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClientApp
{
    internal class TestTask
    {
        static string api_test = "https://localhost:7162/api/tasks/";

        public static async Task<List<string>> GetCadsAsync(int nCards = 5)
        {
            var cards = await Task.Run(() =>
            {
                List<string> cards = new List<string>();
                for (int i = 0; i < nCards; i++)
                {
                    cards.Add(i.ToString().PadLeft(16, '0'));
                }
                return cards;
            });

            return cards;
        }

        public static async Task<List<CardProcessed>> ProcessCardsAsync(List<string> cards)
        {
            var tasks = new List<Task<HttpResponseMessage>>();
            var cardsResult = new List<CardProcessed>();
            using var httpClient = new HttpClient();

            await Task.Run(() =>
            {
                foreach (string card in cards)
                {
                    var json = JsonConvert.SerializeObject(card);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var responseTask = httpClient.PostAsync($"{api_test}processCard", content);
                    tasks.Add(responseTask);
                }
            });

            await Task.WhenAll(tasks);
            foreach(var task in tasks)
            {                
                var t = await task;
                var content = await t.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CardProcessed>(content);
                cardsResult.Add(result!);
            }

            return cardsResult;
        }
    }
}
