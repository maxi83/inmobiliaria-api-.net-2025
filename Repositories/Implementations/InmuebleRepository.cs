using Inmobiliaria_api_mobile.Data;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria_api_mobile.Repositories.Implementations;

public class InmuebleRepository(AppCntxt context) : IInmuebleRepository
{
    private readonly AppCntxt _context = context;

    public async Task<Inmueble> AddAsync(Inmueble inmueble)
    {
        _context.Inmuebles.Add(inmueble);
        await _context.SaveChangesAsync();
        return inmueble;
    }

    public async Task<Inmueble?> GetByIdAsync(int id)
    {
        return await _context.Inmuebles.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IReadOnlyList<Inmueble>> GetByPropietarioAsync(int propietarioId)
    {
        return await _context
            .Inmuebles.Where(i => i.PropietarioId == propietarioId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Inmueble inmueble)
    {
        _context.Inmuebles.Update(inmueble);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Inmuebles.FindAsync(id);
        if (entity is null)
            return;

        _context.Inmuebles.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
