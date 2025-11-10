using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inmobiliaria_api_mobile.DTOs;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Inmobiliaria_api_mobile.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentaController(IPropietarioRepository repo, IConfiguration config) : ControllerBase
{
    private readonly IPropietarioRepository _repo = repo;
    private readonly IConfiguration _config = config;
    private readonly PasswordHasher<Propietario> _hasher = new();

    // Registro de propietario
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO nuevo)
    {
        if (await _repo.EmailExistsAsync(nuevo.Email))
            return BadRequest("El email ya está registrado");

        var propietario = new Propietario
        {
            Email = nuevo.Email,
            Nombre = nuevo.Nombre,
            Apellido = nuevo.Apellido,
            DNI = nuevo.DNI,
            Telefono = nuevo.Telefono,
        };

        propietario.PasswordHash = _hasher.HashPassword(propietario, nuevo.Password);
        await _repo.AddAsync(propietario);

        return Ok("Registro exitoso");
    }

    // Login de propietario
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        var propietario = await _repo.GetByEmailAsync(login.Email);
        if (propietario == null)
            return Unauthorized("Usuario no encontrado");

        var result = _hasher.VerifyHashedPassword(propietario, propietario.PasswordHash, login.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Contraseña incorrecta");

        // Generar token JWT
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, propietario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, propietario.Email),
            new Claim("nombre", propietario.Nombre)
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

    // Cambiar contraseña
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
    {
        string email = User.FindFirstValue(JwtRegisteredClaimNames.Email) ?? "";
        var propietario = await _repo.GetByEmailAsync(email);
        if (propietario == null)
            return NotFound("Usuario no encontrado");

        var result = _hasher.VerifyHashedPassword(propietario, propietario.PasswordHash, model.OldPassword);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Contraseña actual incorrecta");

        propietario.PasswordHash = _hasher.HashPassword(propietario, model.NewPassword);
        await _repo.UpdateAsync(propietario);

        return Ok("Contraseña actualizada");
    }

    // Ver perfil
    [HttpGet("ver-perfil")]
    [Authorize]
    public async Task<IActionResult> VerPerfil()
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var propietario = await _repo.GetByIdAsync(propietarioId);
        if (propietario == null)
            return NotFound("No se encontró el propietario");

        return Ok(propietario);
    }

    // Editar perfil
    [HttpPut("editar-perfil")]
    [Authorize]
    public async Task<IActionResult> EditarPerfil([FromBody] UpdateProfileDTO model)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var propietario = await _repo.GetByIdAsync(propietarioId);
        if (propietario == null)
            return NotFound("No se encontró el propietario");

        propietario.Nombre = model.Nombre;
        propietario.Apellido = model.Apellido;
        propietario.DNI = model.DNI;
        propietario.Telefono = model.Telefono;

        await _repo.UpdateAsync(propietario);
        return Ok(propietario);
    }
}
