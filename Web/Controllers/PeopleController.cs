using Home.Source.BusinessLayer;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.DTOs;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleLayer peopleLayer;
        private readonly IOutputCacheStore outputCacheStore;

        public PeopleController(PeopleLayer peopleLayer, IOutputCacheStore outputCacheStore)
        {
            this.peopleLayer = peopleLayer;
            this.outputCacheStore = outputCacheStore;
        }

        [HttpGet]
        //[OutputCache(Duration = 60)]
        [OutputCache(PolicyName = "Policy-People-Get")]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            await Task.Delay(2000);
            return await peopleLayer.GetPeopleAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePerson(PersonDTO dto)
        { 
            await peopleLayer.CreatePersonAsync(dto);
            await outputCacheStore.EvictByTagAsync("People-Get", default);
            return Ok();
        }
    }
}
