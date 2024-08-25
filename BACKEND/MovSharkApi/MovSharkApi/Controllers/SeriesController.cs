using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MovSharkApi.Services;

namespace MovSharkApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly IDbConfigurationService _dbConfigService;

        public SeriesController(IDbConfigurationService dbConfigService)
        {
            _dbConfigService = dbConfigService;
        }

        [HttpGet("series")]
        public async Task<IActionResult> GetSeries()
        {
            try
            {
                var connectionString = _dbConfigService.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                var result = await connection.QueryAsync("SELECT * FROM Series");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "ERROR: " + ex.Message });

            }
        }
    }
}
