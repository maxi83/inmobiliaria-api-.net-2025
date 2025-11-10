using Microsoft.AspNetCore.Http;
using Inmobiliaria_api_mobile.Models;
using inmobiliaria_api_mobile.Enums;
namespace inmobiliaria_api_mobile.DTOs;

public class CrearInmuebleDTO
{
    public string Direccion { get; set; } = "";
    public Uso Uso { get; set; }
    public Tipo Tipo { get; set; }
    public int NoAmbientes { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public decimal Precio { get; set; }
    public Disponibilidad Disponibilidad { get; set; }
    public IFormFile? Imagen { get; set; }
    public string? filename { get; set; }
}