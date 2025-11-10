using Inmobiliaria_api_mobile.Models;
namespace inmobiliaria_api_mobile.DTOs;

public class ContratosYPagosDTO {
    public Contrato Contrato { get; set; } = null!;
    public IReadOnlyList<Pago> Pagos { get; set; } = null!;
}
