using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MovSharkApi.Services;

namespace MovSharkApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IDbConfigurationService _dbConfigService;

        public FavoritesController(IDbConfigurationService dbConfigService)
        {
            _dbConfigService = dbConfigService;
        }

        // ver todos
        [HttpGet("favorites")]
        public async Task<IActionResult> GetAllFavorites()
        {
            try
            {
                var connectionString = _dbConfigService.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                var favorites = await connection.QueryAsync("SELECT * FROM Favoritos");
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //agregar
        [HttpPost("favorites")]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteRequest request)
        {
            try
            {
                var connectionString = _dbConfigService.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                string sql;
                if (request.Type == "movie")
                {
                    sql = "INSERT INTO Favoritos (usuarioId, peliculaId) VALUES (@UsuarioId, @PeliculaId)";
                }
                else
                {
                    sql = "INSERT INTO Favoritos (usuarioId, serieId) VALUES (@UsuarioId, @SerieId)";
                }
                await connection.ExecuteAsync(sql, request);
                return StatusCode(201, new { message = "Favorito añadido correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //ver favoritos
        [HttpGet("favorites/{usuarioId}")]
        public async Task<IActionResult> GetFavorites(int usuarioId)
        {
            try
            {
                var connectionString = _dbConfigService.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                var sql = "SELECT * FROM Favoritos WHERE usuarioId = @UsuarioId";
                var favorites = await connection.QueryAsync<FavoriteRequest>(sql, new { UsuarioId = usuarioId });
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //eliminar
        [HttpDelete("favorites")]
        public async Task<IActionResult> RemoveFavorite([FromBody] FavoriteRemoveRequest request)
        {
            try
            {
                var connectionString = _dbConfigService.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                var sql = "DELETE FROM Favoritos WHERE usuarioId = @UsuarioId AND (peliculaId = @PeliculaId OR serieId = @SerieId)";
                await connection.ExecuteAsync(sql, request);
                return Ok(new { message = "Favorito eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
