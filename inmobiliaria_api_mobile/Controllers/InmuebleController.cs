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
public class InmueblesController(IInmuebleRepository repo, IWebHostEnvironment env) : ControllerBase
{
    private readonly IInmuebleRepository _repo = repo;
    private readonly IWebHostEnvironment _env = env;

    // Crear inmueble
    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromForm] Inmueble inmueble,
        [FromForm] IFormFile imagen
    )
    {
        foreach (var claim in User.Claims)
            Console.WriteLine($"CLAIM: {claim.Type} = {claim.Value}");

        // var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        inmueble.PropietarioId = propietarioId;

        if (imagen == null || imagen.Length == 0)
            return BadRequest("No se recibió ninguna imagen.");

        string filename = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
        var ruta = Path.Combine(_env.WebRootPath, "imagenes", filename);

        using (var stream = new FileStream(ruta, FileMode.Create))
        {
            await imagen.CopyToAsync(stream);
        }

        inmueble.Foto = filename;
        var creado = await _repo.AddAsync(inmueble);
        return Ok(creado);
    }

    // Listar inmuebles del propietario autenticado
    [HttpGet]
    public async Task<IActionResult> MisInmuebles()
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var inmuebles = await _repo.GetByPropietarioAsync(propietarioId);
        foreach (Inmueble inmueble in inmuebles)
        {
            inmueble.Foto = Path.Combine(_env.WebRootPath, "imagenes", inmueble.Foto);
        }
        return Ok(inmuebles);
    }

    // Obtener un inmueble específico
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var inmueble = await _repo.GetByIdAsync(id);

        if (inmueble == null || inmueble.PropietarioId != propietarioId)
            return NotFound();

        inmueble.Foto = Path.Combine(_env.WebRootPath, "imagenes", inmueble.Foto);

        return Ok(inmueble);
    }

    // Actualizar inmueble
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Inmueble inmueble)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (inmueble.PropietarioId != propietarioId || id != inmueble.Id)
            return Unauthorized("No puedes modificar inmuebles de otro propietario");

        await _repo.UpdateAsync(inmueble);
        return Ok(inmueble);
    }

    // Eliminar inmueble
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var inmueble = await _repo.GetByIdAsync(id);

        if (inmueble == null || inmueble.PropietarioId != propietarioId)
            return Unauthorized("No puedes eliminar inmuebles de otro propietario");

        await _repo.DeleteAsync(id);
        return Ok("Inmueble eliminado");
    }
}
