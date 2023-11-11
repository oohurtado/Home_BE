using Home.Source.BusinessLayer;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.DTOs;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleLayer peopleLayer;

        public PeopleController(PeopleLayer peopleLayer)
        {
            this.peopleLayer = peopleLayer;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            return await peopleLayer.GetPeopleAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePerson(PersonDTO dto)
        { 
            await peopleLayer.CreatePersonAsync(dto);
            return Ok();
        }
    }
}
