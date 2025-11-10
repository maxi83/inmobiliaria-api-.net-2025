namespace Inmobiliaria_api_mobile.DTOs;

    public class RegisterDTO {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int DNI { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Telefono { get; set; }
    }
