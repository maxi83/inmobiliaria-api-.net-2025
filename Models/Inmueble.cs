using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria_api_mobile.Models;

public class Inmueble
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Propietario")]
    public int PropietarioId { get; set; }
    public virtual required Propietario? Propietario { get; set; }
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

public enum Uso
{
    COMERCIAL,
    RESIDENCIAL,
}

public enum Tipo
{
    LOCAL,
    DEPOSITO,
    CASA,
    DEPARTAMENTO,
}

public enum Disponibilidad
{
    OCUPADO,
    SUSPENDIDO,
    DESOCUPADO,
}
