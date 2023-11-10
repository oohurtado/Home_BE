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
        public async Task<ActionResult<List<TestDTO>>> GetData()
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
    }
}
