using Inmobiliaria_api_mobile.Data;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria_api_mobile.Repositories.Implementations;

public class InquilinoRepository(AppCntxt context) : IInquilinoRepository
{
    private readonly AppCntxt _context = context;

    public async Task<Inquilino> AddAsync(Inquilino inquilino)
    {
        await _context.Inquilinos.AddAsync(inquilino);
        await _context.SaveChangesAsync();
        return inquilino;
    }

    public async Task<Inquilino?> GetByIdAsync(int id)
    {
        return await _context.Inquilinos.FindAsync(id);
    }

    public async Task UpdateAsync(Inquilino inquilino)
    {
        _context.Inquilinos.Update(inquilino);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Inquilino? inquilino = await _context.Inquilinos.FindAsync(id);
        if (inquilino != null)
        {
            _context.Inquilinos.Remove(inquilino);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Inquilinos.AnyAsync(i => i.Id == id);
    }
}
