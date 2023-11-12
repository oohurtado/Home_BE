using Home.Source.Data.Repositories;
using Home.Source.DataBase;
using Home.Source.Models;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Shared.DTOs;
using System.Data;

namespace Home.Controllers
{
    public partial class SQLController : ControllerBase
    {
        [HttpPost(template: "create-person")]
        public async Task<ActionResult<int>> CreatePerson(PersonDTO personDTO)
        {
            /*
             * sp_InsertPerson, @p_firstname NVARCHAR(25), @p_lastname NVARCHAR(25), @out_id INT OUTPUT
             */
            var parameterId = new SqlParameter("@out_id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await databaseContext
                .Database
                .ExecuteSqlInterpolatedAsync($@"EXEC sp_InsertPerson @p_firstname={personDTO.FirstName}, @p_lastname={personDTO.LastName}, @out_id={parameterId} OUTPUT");

            var id = (int)parameterId.Value;
            return id;
        }

        [HttpGet(template: "getPersonById")]
        public async Task<ActionResult<Person>> GetPersonById(int id)
        {
            /*
             * sp_GetById, @p_id
             */
            var people = databaseContext
                .People
                .FromSqlInterpolated($"EXEC sp_GetById @p_id={id}")
                .AsAsyncEnumerable();

            await foreach(var person in people)
            {
                return person;
            }

            return NotFound();
        } 

        /*
         * sp_GetIds
         */
        [HttpGet(template: "getPeopleIds")]
        public async Task<IEnumerable<PersonIdResult>> GetPeopleIds()
        {
            return await databaseContext.Set<PersonIdResult>().ToListAsync();
        }

        [HttpGet(template: "getPeople")]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            /*
             * sp_GetById, @p_id
             */
            var people = await databaseContext.People.FromSqlRaw("sp_GetPeople").ToListAsync();
            return people;
        }
    }
}
