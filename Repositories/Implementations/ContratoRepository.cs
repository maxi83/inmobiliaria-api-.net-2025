using Inmobiliaria_api_mobile.Data;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria_api_mobile.Repositories.Implementations;

public class ContratoRepository(AppCntxt context) : IContratoRepository
{
    private readonly AppCntxt _context = context;

    public async Task<Contrato> AddAsync(Contrato contrato)
    {
        _context.Contratos.Add(contrato);
        await _context.SaveChangesAsync();
        return contrato;
    }

    public async Task<Contrato?> GetByIdAsync(int id)
    {
        return await _context
            .Contratos.Include(c => c.Inquilino)
            .Include(c => c.Inmueble)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IReadOnlyList<Contrato>> GetByInmuebleAsync(int inmuebleId)
    {
        return await _context
            .Contratos.Where(c => c.InmuebleId == inmuebleId)
            .Include(c => c.Inquilino)
            .Include(c => c.Inmueble)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Contrato>> GetByRangoFechasAsync(DateOnly desde, DateOnly hasta)
    {
        return await _context
            .Contratos.Where(c => c.Desde >= desde && c.Hasta <= hasta)
            .Include(c => c.Inquilino)
            .Include(c => c.Inmueble)
            .ToListAsync();
    }

    public async Task UpdateAsync(Contrato contrato)
    {
        _context.Contratos.Update(contrato);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var contrato = await _context.Contratos.FindAsync(id);
        if (contrato is not null)
        {
            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Contratos.AnyAsync(c => c.Id == id);
    }
}
