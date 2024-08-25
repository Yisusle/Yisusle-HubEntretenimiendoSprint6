using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MovSharkApi.Services;

namespace MovSharkApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDbConfigurationService _dbConfigService;

        public UsersController(IDbConfigurationService dbConfigService)
        {
            _dbConfigService = dbConfigService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var connectionString = _dbConfigService.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                var result = await connection.QuerySingleOrDefaultAsync("SELECT * FROM Usuarios WHERE Correo = @Correo", new { request.Correo });

                if (result != null)
                {
                    var isMatch = BCrypt.Net.BCrypt.Verify(request.Contraseña, result.Contraseña);

                    if (isMatch)
                    {
                        return Ok(new { message = "Login Correcto", user = result });
                    }
                    else
                    {
                        return Unauthorized(new { message = "Email o Contraseña Incorrectos" });
                    }
                }
                else
                {
                    return Unauthorized(new { message = "Email o Contraseña Incorrectos" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistroRequest request)
        {
            try
            {
                var connectionString = _dbConfigService.GetConnectionString();
                using var connection = new SqlConnection(connectionString);

                // Verificar si el usuario ya existe
                var existingUser = await connection.QuerySingleOrDefaultAsync("SELECT * FROM Usuarios WHERE Correo = @Correo", new { request.Correo });
                if (existingUser != null)
                {
                    return Conflict(new { message = "El usuario ya existe" });
                }

                // Hashear la contraseña antes de guardarla en la base de datos
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Contraseña);

                // Insertar el nuevo usuario en la base de datos
                var sql = "INSERT INTO Usuarios (Nombre, Apellido, Correo, Contraseña) VALUES (@Nombre, @Apellido, @Correo, @Contraseña)";
                var rowsAffected = await connection.ExecuteAsync(sql, new { request.Nombre, request.Apellido, request.Correo, Contraseña = hashedPassword });

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "Usuario registrado correctamente" });
                }

                return StatusCode(500, new { message = "No se pudo registrar el usuario" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
