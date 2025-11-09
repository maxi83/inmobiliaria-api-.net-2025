using Inmobiliaria_api_mobile.Models;

namespace Inmobiliaria_api_mobile.Repositories.Interfaces;

public interface IPagoRepository
{
    Task<IReadOnlyList<Pago>> GetByContratoIdAsync(int contratoId);

    Task<Pago> AddAsync(Pago pago);
    Task<Pago?> GetByIdAsync(int id);
    Task UpdateAsync(Pago pago);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);

    Task<IEnumerable<(int Id, string NombreCompleto)>> GetInquilinosLookupAsync();
    Task<IEnumerable<(int Id, string Direccion)>> GetContratosLookupAsync();
}
