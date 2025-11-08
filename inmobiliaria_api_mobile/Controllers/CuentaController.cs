using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Inmobiliaria_api_mobile.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentaController(IPropietarioRepository repo, IConfiguration config) : ControllerBase
{
    private readonly IPropietarioRepository _repo = repo;

    // IConfiguration es el archivo appsettings.json.
    private readonly IConfiguration _config = config;
    private readonly PasswordHasher<Propietario> _hasher = new();

    // Registro de propietario
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Propietario nuevo)
    {
        if (await _repo.EmailExistsAsync(nuevo.Email))
            return BadRequest("El email ya está registrado");

        // Hashear la contraseña antes de guardar
        nuevo.PasswordHash = _hasher.HashPassword(nuevo, nuevo.PasswordHash);

        await _repo.AddAsync(nuevo);

        return Ok("Registro exitoso");
    }

    // Login de propietario
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Propietario login)
    {
        var propietario = await _repo.GetByEmailAsync(login.Email);
        if (propietario == null)
            return Unauthorized("Usuario no encontrado");

        var result = _hasher.VerifyHashedPassword(
            propietario,
            propietario.PasswordHash,
            login.PasswordHash
        );
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Contraseña incorrecta");

        // Claims para el token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, propietario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, propietario.Email),
            new Claim("nombre", propietario.Nombre),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
