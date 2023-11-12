using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpGet(template: "test1")]
        public async Task<ActionResult<Tuple<List<string>, List<int>>>> Test1()
        {
            var c1 = CallStrings();
            var c2 = CallInts();
            await Task.WhenAll(c1, c2);
            var r1 = c1.Result;
            var r2 = c2.Result;

            return Ok(new Tuple<List<string>, List<int>>(r1,r2));
        }

        async Task<List<string>> CallStrings()
        {
            await Task.Delay(3 * 1000);
            return new List<string>()
            {
                "Aaa",
                "Bbb",
                "Ccc",
            };
        }

        async Task<List<int>> CallInts()
        {
            await Task.Delay(3 * 1000);
            return new List<int>()
            {
                111,
                222,
                333
            };
        }

        // https://www.youtube.com/watch?v=7eAl_4Pxc2s
        [HttpPost(template: "processCard")]
        public async Task<ActionResult> ProcessCard([FromBody] string card)
        {
            var random = new Random().Next(1, 10);
            bool rejected = random % 2 == 0;
            await Task.Delay(1000);
            await Console.Out.WriteLineAsync($"Prosessing card: {card}");
            return Ok(new CardProcessed { Card = card, Rejected = rejected });
        }


    }
}
