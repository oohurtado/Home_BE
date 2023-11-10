using Home.Source.Data.Repositories;
using Home.Source.DataBase;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SQLController : ControllerBase
    {
        private string connectionString;

        public SQLController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // llama a sp enviando listado de enteros
        [HttpGet(template: "getPeople")]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            var people = new List<Person>();
            var ids = new List<int>() { 62, 64 };

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using SqlCommand command = new SqlCommand("sp_GetByIdList", connection);
                command.CommandType = CommandType.StoredProcedure;

                var dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));
                ids.ForEach(i => dt.Rows.Add(i));

                var parameter = command.Parameters.AddWithValue("p_ids", dt);
                parameter.SqlDbType = SqlDbType.Structured;

                var reader = await command.ExecuteReaderAsync();
                while(await reader.ReadAsync())
                {
                    var data = reader["PersonId"];
                    people.Add(new Person()
                    {
                        Id = int.Parse(reader["PersonId"].ToString()!),
                        FirstName = reader["FirstName"].ToString()!,
                        LastName = reader["LastName"].ToString()!,
                    });
                }
            }
            
            return Ok(people);
        }
    }
}
