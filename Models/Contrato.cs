using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Inmobiliaria_api_mobile.Models;

namespace Inmobiliaria_api_mobile.Models;

public class Contrato
{
    public Contrato() { }

    [Key]
    public int Id { get; set; }
    public DateOnly Desde { get; set; }
    public DateOnly Hasta { get; set; }

    [ForeignKey("Inquilino")]
    public int InquilinoId { get; set; }
    public virtual required Inquilino? Inquilino { get; set; }

    [ForeignKey("Inmueble")]
    public int InmuebleId { get; set; }
    public virtual required Inmueble? Inmueble { get; set; }

    [InverseProperty("Contrato")]
    public virtual required List<Pago>? Pagos { get; set; }
}
