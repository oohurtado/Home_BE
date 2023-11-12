using Home.Source.Data;
using Home.Source.Data.Repositories;
using Home.Source.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class SQLController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        private string connectionString;

        public SQLController(IConfiguration configuration, DatabaseContext databaseContext)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
            this.databaseContext = databaseContext;
        }
    }
}
