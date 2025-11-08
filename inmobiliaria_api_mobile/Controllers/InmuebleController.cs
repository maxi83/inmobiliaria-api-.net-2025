using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_api_mobile.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InmueblesController(IInmuebleRepository repo) : ControllerBase
{
    private readonly IInmuebleRepository _repo = repo;

    // Crear inmueble
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Inmueble inmueble)
    {
        var propietarioId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        inmueble.PropietarioId = propietarioId;

        var creado = await _repo.AddAsync(inmueble);
        return Ok(creado);
    }

    // Listar inmuebles del propietario autenticado
    [HttpGet]
    public async Task<IActionResult> MisInmuebles()
    {
        var propietarioId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var inmuebles = await _repo.GetByPropietarioAsync(propietarioId);
        return Ok(inmuebles);
    }

    // Obtener un inmueble específico
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var propietarioId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var inmueble = await _repo.GetByIdAsync(id);

        if (inmueble == null || inmueble.PropietarioId != propietarioId)
            return NotFound();

        return Ok(inmueble);
    }

    // Actualizar inmueble
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Inmueble inmueble)
    {
        var propietarioId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        if (inmueble.PropietarioId != propietarioId || id != inmueble.Id)
            return Unauthorized("No puedes modificar inmuebles de otro propietario");

        await _repo.UpdateAsync(inmueble);
        return Ok(inmueble);
    }

    // Eliminar inmueble
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var propietarioId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var inmueble = await _repo.GetByIdAsync(id);

        if (inmueble == null || inmueble.PropietarioId != propietarioId)
            return Unauthorized("No puedes eliminar inmuebles de otro propietario");

        await _repo.DeleteAsync(id);
        return Ok("Inmueble eliminado");
    }
}
