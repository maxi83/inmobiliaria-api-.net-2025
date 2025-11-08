using Inmobiliaria_api_mobile.Data;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria_api_mobile.Repositories.Implementations;

public class PagoRepository(AppCntxt context) : IPagoRepository
{
    private readonly AppCntxt _context = context;

    public async Task<IReadOnlyList<Pago>> GetByContratoIdAsync(int contratoId)
    {
        List<Pago> pagos = await _context
            .Pagos.Where(p => p.ContratoId == contratoId)
            .OrderBy(p => p.NoPago)
            .ToListAsync();

        return pagos;
    }

    public async Task<Pago> AddAsync(Pago pago)
    {
        await _context.Pagos.AddAsync(pago);
        await _context.SaveChangesAsync();
        return pago;
    }

    public async Task<Pago?> GetByIdAsync(int id)
    {
        return await _context.Pagos.FindAsync(id);
    }

    public async Task UpdateAsync(Pago pago)
    {
        _context.Pagos.Update(pago);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Pago? pago = await _context.Pagos.FindAsync(id);
        if (pago != null)
        {
            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Pagos.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<(int Id, string NombreCompleto)>> GetInquilinosLookupAsync()
    {
        List<(int Id, string NombreCompleto)> resultado = await _context
            .Inquilinos.OrderBy(i => i.Apellido)
            .ThenBy(i => i.Nombre)
            .Select(i => new ValueTuple<int, string>(i.Id, $"{i.Apellido}, {i.Nombre}"))
            .ToListAsync();

        return resultado;
    }

    public async Task<IEnumerable<(int Id, string Direccion)>> GetContratosLookupAsync()
    {
        List<(int Id, string Direccion)> resultado = await _context
            .Contratos.Include(c => c.Inmueble)
            .OrderBy(c => c.Inmueble.Direccion)
            .Select(c => new ValueTuple<int, string>(c.Id, c.Inmueble.Direccion))
            .ToListAsync();

        return resultado;
    }
}
