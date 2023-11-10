using Hangfire;
using Home.Source.BusinessLayer;
using Home.Source.Data.Infrastructure;
using Home.Source.Data.Repositories;
using Home.Source.DataBase;
using Home.Source.Models;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Xml;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestHttpController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogRepository logRepository;
        private readonly IBackgroundJobClient backgroundJobClient;

        public TestHttpController(DatabaseContext databaseContext, IServiceProvider serviceProvider, ILogRepository logRepository, IBackgroundJobClient backgroundJobClient)
        {
            this.databaseContext = databaseContext;
            this.serviceProvider = serviceProvider;
            this.logRepository = logRepository;
            this.backgroundJobClient = backgroundJobClient;
        }

        [HttpGet(template: "getData")]
        public async Task<ActionResult<List<TestDTO>>> getData()
        {
            await Task.Delay(1);
            return Ok(new List<TestDTO>()
            {
                new TestDTO
                {
                    FirstName = "Oscar",
                    LastName = "Hurtado",
                }
            });
        }

        [HttpPost(template: "createData")]
        public async Task<ActionResult<TestDTO>> CreateData([FromBody] TestDTO dto)
        {
            await Task.Delay(1);
            return Ok("yay");
        }

        [HttpPut(template: "updateData/{id}")]
        public async Task<ActionResult> UpdateData(int id, [FromBody] TestDTO dto)
        {
            await Task.Delay(1);
            return Ok();
        }

        [HttpDelete(template: "deleteData/{id}")]
        public async Task<ActionResult> DeleteData(int id)
        {
            await Task.Delay(1);
            return Ok();
        }

        [HttpGet(template: "getDummy")]
        public async Task<ActionResult<DummyDTO>> GetDummy([FromHeader] int xxx = 5)
        {
            await Task.Delay(1);
            return Ok(new DummyDTO 
            {
                STR = xxx.ToString(),
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(template: "getDummySafe")]
        public async Task<ActionResult<DummyDTO>> GetDummySafe()
        {
            await Task.Delay(1);
            return Ok(new DummyDTO
            {
                STR = "Safe",
            });
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
        
        /// <summary>
        /// Envia ok antes de terminar salvado de logs
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "forget2")]
        public async Task<ActionResult> Forget2()
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
            backgroundJobClient.ContinueJobWith(jobId,() => Console.WriteLine($"El job con id: {jobId} ha concluido"));
            return Ok("yay");
        }
    }
}
