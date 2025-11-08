using Inmobiliaria_api_mobile.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria_api_mobile.Data;

public class AppCntxt(DbContextOptions<AppCntxt> options) : DbContext(options)
{
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<Inmueble> Inmuebles { get; set; }
}
