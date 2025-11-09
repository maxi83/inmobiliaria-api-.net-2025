using Inmobiliaria_api_mobile.Models;

namespace Inmobiliaria_api_mobile.Repositories.Interfaces;

public interface IContratoRepository
{
    Task<Contrato> AddAsync(Contrato contrato);
    Task<Contrato?> GetByIdAsync(int id);
    Task<IReadOnlyList<Contrato>> GetByInmuebleAsync(int InmuebleId);
    Task UpdateAsync(Contrato contrato);
    Task DeleteAsync(int id);
    Task<IReadOnlyList<Contrato>> GetByRangoFechasAsync(DateOnly desde, DateOnly hasta);
    Task<bool> ExistsAsync(int id);
}
