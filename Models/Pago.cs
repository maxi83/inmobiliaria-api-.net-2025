using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria_api_mobile.Models;

public class Pago
{
    public Pago() { }

    [Key]
    public int Id { get; set; }
    public int NoPago { get; set; }
    public DateOnly Fecha { get; set; }
    public decimal Importe { get; set; }

    [ForeignKey("Contrato")]
    public int ContratoId { get; set; }
    public virtual required Contrato? Contrato { get; set; }

    [ForeignKey("Inquilino")]
    public int InquilinoId { get; set; }
    public virtual Inquilino? Inquilino { get; set; }
}
