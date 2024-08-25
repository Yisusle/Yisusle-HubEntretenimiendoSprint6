using System.ComponentModel.DataAnnotations;

namespace MovSharkApi
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [MinLength(5)]
        public string Contraseña { get; set; }
    }

    public class RegistroRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
    }
}
