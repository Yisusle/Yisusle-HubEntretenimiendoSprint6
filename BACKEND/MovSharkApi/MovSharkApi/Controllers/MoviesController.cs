using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Extensions.Configuration;
using MovSharkApi.Services;
using Microsoft.Data.SqlClient;

namespace MovSharkApi.Controllers
{
    [ApiController]
    [Route("api/")]
    
    public class MoviesController : ControllerBase
    {

        private readonly IDbConfigurationService _dbConfigService;

        public MoviesController(IDbConfigurationService dbConfigService)
        {
            _dbConfigService = dbConfigService;
        }

        [HttpGet("movies")]
        public async Task<IActionResult> GetMovies()
        {
            try
            {
                var connectionString = _dbConfigService.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                var result = await connection.QueryAsync("SELECT * FROM Peliculas");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "ERROR: " + ex.Message });
            }
        }
    }
}
