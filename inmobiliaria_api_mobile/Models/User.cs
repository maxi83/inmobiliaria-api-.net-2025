using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;

namespace Inmobiliaria_api_mobile.Models;

public class User
{
    public User() { }

    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Rol { get; set; } = "";
}
