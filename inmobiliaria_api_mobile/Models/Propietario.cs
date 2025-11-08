using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria_api_mobile.Models;

public class Propietario
{
    [Key]
    public int Id { get; set; }

    [InverseProperty("Propietario")]
    public virtual List<Inmueble>? Inmuebles { get; set; }
    public int DNI { get; set; }
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Email { get; set; } = "";
    public int Telefono { get; set; }
    public string PasswordHash { get; set; } = "";
}
