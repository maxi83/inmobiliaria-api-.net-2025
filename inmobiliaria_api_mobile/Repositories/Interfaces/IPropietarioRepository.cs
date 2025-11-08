using Inmobiliaria_api_mobile.Models;

namespace Inmobiliaria_api_mobile.Repositories.Interfaces;

public interface IPropietarioRepository
{
    // Registro
    Task<Propietario> AddAsync(Propietario propietario);

    // Consultas
    Task<Propietario?> GetByIdAsync(int id);
    Task<Propietario?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);

    // Actualización y borrado
    Task UpdateAsync(Propietario propietario);
    Task DeleteAsync(int id);

    // Listado (si necesitas administración)
    Task<IReadOnlyList<Propietario>> GetAllAsync();
}
