using Inmobiliaria_api_mobile.Models;

namespace Inmobiliaria_api_mobile.Repositories.Interfaces;

public interface IInmuebleRepository
{
    Task<Inmueble> AddAsync(Inmueble inmueble);
    Task<Inmueble?> GetByIdAsync(int id);
    Task<IReadOnlyList<Inmueble>> GetByPropietarioAsync(int propietarioId);
    Task UpdateAsync(Inmueble inmueble);
    Task DeleteAsync(int id);
}
