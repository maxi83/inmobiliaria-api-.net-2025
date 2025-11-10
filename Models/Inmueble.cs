using inmobiliaria_api_mobile.Enums;
using inmobiliaria_api_mobile.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria_api_mobile.Models;

public class Inmueble
{
    public Inmueble() { }
    public Inmueble(CrearInmuebleDTO dto)
    {
        Direccion = dto.Direccion;
        Uso = (Uso)dto.Uso;
        Tipo = (Tipo)dto.Tipo;
        NoAmbientes = dto.NoAmbientes;
        Latitud = dto.Latitud;
        Longitud = dto.Longitud;
        Precio = dto.Precio;
        Disponibilidad = (Disponibilidad)dto.Disponibilidad;
        Foto = dto.filename!;
    }

    [Key]
    public int Id { get; set; }

    [ForeignKey("Propietario")]
    public int PropietarioId { get; set; }
    public virtual Propietario? Propietario { get; set; }
    public string Direccion { get; set; } = "";
    public Uso Uso { get; set; }
    public Tipo Tipo { get; set; }
    public int NoAmbientes { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public decimal Precio { get; set; }
    public Disponibilidad Disponibilidad { get; set; }
    public string Foto { get; set; } = "";
}