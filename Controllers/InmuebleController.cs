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
public class InmueblesController(IInmuebleRepository repo, IWebHostEnvironment env, IContratoRepository repoContrato, IPagoRepository repoPago) : ControllerBase
{
    private readonly IInmuebleRepository _repo = repo;
    private readonly IContratoRepository _repoContrato = repoContrato;
    private readonly IPagoRepository _repoPago = repoPago;
    private readonly IWebHostEnvironment _env = env;

    // Crear inmueble
    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromForm] Inmueble inmueble,
        [FromForm] IFormFile imagen
    )
    {
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

    [HttpPost]
    public async Task<IActionResult> CambiarDisponibilidad([FromBody] int Id, Disponibilidad disponibilidad)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Inmueble? inmueble = await _repo.GetByIdAsync(Id);

        if (inmueble?.PropietarioId != propietarioId || Id != inmueble?.Id)
            return Unauthorized("No puedes modificar inmuebles de otro propietario");
        inmueble.Disponibilidad = disponibilidad;
        await _repo.UpdateAsync(inmueble);
        return Ok(inmueble);
    }
    [HttpPost]
    public async Task<IActionResult> ContratosPorInmuebleYPagos([FromBody] int InmuebleId)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        List<(Contrato, IReadOnlyList<Pago>)> contrataciones = [];
        IReadOnlyList<Contrato> Contratos = await _repoContrato.GetByInmuebleAsync(InmuebleId);
        foreach (Contrato contrato in Contratos)
        {
            IReadOnlyList<Pago> Pagos = await _repoPago.GetByContratoIdAsync(contrato.Id);
            contrataciones.Add((contrato, Pagos));
        }
        return Ok(contrataciones);
    }
}
