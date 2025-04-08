using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Crudt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SuperHeroController(IConfiguration  config)
        {
              _config =config ;
        }
        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeros()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<SuperHero> heros = await SelectAllHeros(connection);
            return Ok(heros);
        }

        private static async Task<IEnumerable<SuperHero>> SelectAllHeros(SqlConnection connection)
        {
            return await connection.QueryAsync<SuperHero>("select * from superheros");
        }

        [HttpGet("{heroid}")]
        public async Task<ActionResult<SuperHero>> GetHero(int heroid)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryFirstAsync<SuperHero>("select * from superheros where id=@id",
                new {id =heroid});
            return Ok(hero);
        }
        [HttpPost]

        public async Task<ActionResult<List<SuperHero>>> CreateHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into Superheros (id,name,firstname,lastname,place) values(@id,@Name,@FirstName,@LastName,@Place)",hero);
            return Ok(await SelectAllHeros(connection));
        }


    }

}
