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
        [HttpGet(template: "getPeople")]
        public async Task<ActionResult<List<PersonDTO>>> getPeople()
        {
            await Task.Delay(1);
            return Ok(new List<PersonDTO>()
            {
                new PersonDTO
                {
                    FirstName = "Oscar",
                    LastName = "Hurtado",
                }
            });
        }

        [HttpPost(template: "createPerson")]
        public async Task<ActionResult<PersonDTO>> CreatePerson([FromBody] PersonDTO dto)
        {
            await Task.Delay(1);
            return Ok("yay");
        }

        [HttpPut(template: "updatePerson/{id}")]
        public async Task<ActionResult> UpdatePerson(int id, [FromBody] PersonDTO dto)
        {
            await Task.Delay(1);
            return Ok();
        }

        [HttpDelete(template: "deletePerson/{id}")]
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
