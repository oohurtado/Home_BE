using Hangfire;
using Home.Source.BusinessLayer;
using Home.Source.Data.Repositories;
using Home.Source.DataBase;
using Home.Source.Models.Entities;
using Home.Source.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shared.DTOs;
using Home.Source.Data.Infrastructure;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        private readonly ILogRepository logRepository;
        private readonly IServiceProvider serviceProvider;
        private readonly IBackgroundJobClient backgroundJobClient;

        public HangfireController(DatabaseContext databaseContext, ILogRepository logRepository, IServiceProvider serviceProvider, IBackgroundJobClient backgroundJobClient)
        {
            this.databaseContext = databaseContext;
            this.logRepository = logRepository;
            this.serviceProvider = serviceProvider;
            this.backgroundJobClient = backgroundJobClient;
        }

        /// <summary>
        /// Envia ok antes de terminar salvado de logs
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "forget1")]
        public async Task<ActionResult> Forget1()
        {
            var person = new Person
            {
                FirstName = "222",
                LastName = "222"
            };
            databaseContext.People.Add(person);
            await databaseContext.SaveChangesAsync();

            _ = Task.Run(async () =>
            {
                try
                {
                    await using (var scope = serviceProvider.CreateAsyncScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                        var log = new Log
                        {
                            Comment = $"Person saved: {person.FirstName}, {person.LastName}",
                        };
                        dbContext.Logs.Add(log);
                        await dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            });

            return Ok();
        }

        /// <summary>
        /// Envia ok antes de terminar salvado de logs
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "forget2")]
        public async Task<ActionResult> Forget2()
        {
            var person = new Person
            {
                FirstName = "111",
                LastName = "111"
            };
            databaseContext.People.Add(person);
            await databaseContext.SaveChangesAsync();

            _ = Task.Run(async () =>
            {
                try
                {
                    await logRepository.SaveLogAsync($"Person saved: {person.FirstName}, {person.LastName}");
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            });

            return Ok();
        }

        [HttpPost(template: "createPerson")]
        public ActionResult<TestDTO> CreatePerson([FromBody] PersonDTO dto)
        {
            //backgroundJobClient.Enqueue(() => Console.WriteLine($"{dto.FirstName} {dto.LastName}") );
            backgroundJobClient.Enqueue<PeopleLayer>(layer => layer.CreatePersonAsync(dto));

            return Ok("yay");
        }

        [HttpPost(template: "schedule")]
        public ActionResult<TestDTO> Schedule([FromBody] PersonDTO dto)
        {
            //backgroundJobClient.Enqueue(() => Console.WriteLine($"{dto.FirstName} {dto.LastName}") );
            var jobId = backgroundJobClient.Schedule<PeopleLayer>(layer => layer.CreatePersonAsync(dto), TimeSpan.FromSeconds(5));
            backgroundJobClient.ContinueJobWith(jobId, () => Console.WriteLine($"El job con id: {jobId} ha concluido"));
            return Ok("yay");
        }
    }
}
