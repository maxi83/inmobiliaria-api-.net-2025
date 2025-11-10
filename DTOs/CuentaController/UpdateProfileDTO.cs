namespace Inmobiliaria_api_mobile.DTOs;

    public class UpdateProfileDTO {
        public int DNI { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Telefono { get; set; }
    }
