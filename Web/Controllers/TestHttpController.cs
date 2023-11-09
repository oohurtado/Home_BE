using Home.Source.BusinessLayer;
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
    }
}
