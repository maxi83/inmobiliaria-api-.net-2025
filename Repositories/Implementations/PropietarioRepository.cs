using Inmobiliaria_api_mobile.Data;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria_api_mobile.Repositories.Implementations;

public class PropietarioRepository(AppCntxt context) : IPropietarioRepository
{
    private readonly AppCntxt _context = context;

    public async Task<Propietario> AddAsync(Propietario propietario)
    {
        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();
        return propietario;
    }

    public async Task<Propietario?> GetByIdAsync(int id)
    {
        // Incluye inmuebles si necesitas navegar la relación al autenticar o mostrar perfil
        return await _context
            .Propietarios.Include(p => p.Inmuebles)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Propietario?> GetByEmailAsync(string email)
    {
        return await _context.Propietarios.FirstOrDefaultAsync(p => p.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Propietarios.AnyAsync(p => p.Email == email);
    }

    public async Task UpdateAsync(Propietario propietario)
    {
        _context.Propietarios.Update(propietario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Propietarios.FindAsync(id);
        if (entity is null)
            return;

        _context.Propietarios.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Propietario>> GetAllAsync()
    {
        return await _context.Propietarios.AsNoTracking().ToListAsync();
    }
}
