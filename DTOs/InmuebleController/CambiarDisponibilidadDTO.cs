using inmobiliaria_api_mobile.Enums;
using Inmobiliaria_api_mobile.Models;
namespace inmobiliaria_api_mobile.DTOs;

public class CambiarDisponibilidadDTO {
    public int InmuebleId { get; set; }
    public Disponibilidad Disponibilidad { get; set; }
}
