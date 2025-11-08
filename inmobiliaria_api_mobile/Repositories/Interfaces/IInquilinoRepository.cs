using Inmobiliaria_api_mobile.Models;

namespace Inmobiliaria_api_mobile.Repositories.Interfaces;

public interface IInquilinoRepository
{
    Task<Inquilino> AddAsync(Inquilino inquilino);
    Task<Inquilino?> GetByIdAsync(int id);
    Task UpdateAsync(Inquilino inquilino);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
