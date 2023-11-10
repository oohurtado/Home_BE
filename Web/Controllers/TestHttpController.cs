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

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestHttpController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogRepository logRepository;

        public TestHttpController(DatabaseContext databaseContext, IServiceProvider serviceProvider, ILogRepository logRepository)
        {
            this.databaseContext = databaseContext;
            this.serviceProvider = serviceProvider;
            this.logRepository = logRepository;
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
        public async Task<ActionResult<TestDTO>> CreatePerson([FromBody] TestDTO dto)
        {
            await Task.Delay(1);
            return Ok("yay");
        }

        [HttpPut(template: "updateData/{id}")]
        public async Task<ActionResult> UpdatePerson(int id, [FromBody] TestDTO dto)
        {
            await Task.Delay(1);
            return Ok();
        }

        [HttpDelete(template: "deleteData/{id}")]
        public async Task<ActionResult> DeletePerson(int id)
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
    }
}
