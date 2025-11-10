using System.Security.Claims;
using Inmobiliaria_api_mobile.Models;
using inmobiliaria_api_mobile.DTOs;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_api_mobile.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InmueblesController(
    IInmuebleRepository repo,
    IWebHostEnvironment env,
    IContratoRepository repoContrato,
    IPagoRepository repoPago
) : ControllerBase
{
    private readonly IInmuebleRepository _repo = repo;
    private readonly IContratoRepository _repoContrato = repoContrato;
    private readonly IPagoRepository _repoPago = repoPago;
    private readonly IWebHostEnvironment _env = env;

    // Crear inmueble
    [HttpPost("crear")]
    public async Task<IActionResult> Crear([FromForm] CrearInmuebleDTO dto)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (dto.Imagen == null || dto.Imagen.Length == 0)
            return BadRequest("No se recibió ninguna imagen.");

        string filename = Guid.NewGuid() + Path.GetExtension(dto.Imagen.FileName);
        dto.filename = filename;
        string ruta = Path.Combine(_env.WebRootPath, "imagenes", filename);

        using (var stream = new FileStream(ruta, FileMode.Create))
        {
            await dto.Imagen.CopyToAsync(stream);
        }

        Inmueble inmueble = new(dto);

        var creado = await _repo.AddAsync(inmueble);
        return Ok(creado);
    }

    // Listar inmuebles del propietario
    [HttpGet("mis-inmuebles")]
    public async Task<IActionResult> MisInmuebles()
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var inmuebles = await _repo.GetByPropietarioAsync(propietarioId);

        foreach (var inmueble in inmuebles)
        {
            inmueble.Foto = Path.Combine(_env.WebRootPath, "imagenes", inmueble.Foto);
        }

        return Ok(inmuebles);
    }

    // Obtener inmueble específico
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
    [HttpPut("editar")]
    public async Task<IActionResult> Editar([FromBody] EditarInmuebleDTO dto)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var inmueble = await _repo.GetByIdAsync(dto.Id);

        if (inmueble == null || inmueble.PropietarioId != propietarioId)
            return Unauthorized("No puedes modificar inmuebles de otro propietario");

        inmueble.Precio = dto.Precio;
        inmueble.Direccion = dto.Direccion;

        await _repo.UpdateAsync(inmueble);
        return Ok(inmueble);
    }

    // Eliminar inmueble
    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var inmueble = await _repo.GetByIdAsync(id);

        if (inmueble == null || inmueble.PropietarioId != propietarioId)
            return Unauthorized("No puedes eliminar inmuebles de otro propietario");

        await _repo.DeleteAsync(id);
        return Ok("Inmueble eliminado");
    }

    // Cambiar disponibilidad
    [HttpPost("cambiar-disponibilidad")]
    public async Task<IActionResult> CambiarDisponibilidad([FromBody] CambiarDisponibilidadDTO dto)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var inmueble = await _repo.GetByIdAsync(dto.InmuebleId);

        if (inmueble == null || inmueble.PropietarioId != propietarioId)
            return Unauthorized("No puedes modificar inmuebles de otro propietario");

        inmueble.Disponibilidad = dto.Disponibilidad;
        await _repo.UpdateAsync(inmueble);
        return Ok(inmueble);
    }

    // Obtener contratos y pagos de un inmueble
    [HttpPost("contratos-y-pagos")]
    public async Task<IActionResult> ContratosYPagos([FromBody] int inmuebleId)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var inmueble = await _repo.GetByIdAsync(inmuebleId);

        if (inmueble == null || inmueble.PropietarioId != propietarioId)
            return Unauthorized("No puedes ver los contratos de otro propietario");

        var contratos = await _repoContrato.GetByInmuebleAsync(inmuebleId);
        var result = new List<ContratosYPagosDTO>();

        foreach (var contrato in contratos)
        {
            var pagos = await _repoPago.GetByContratoIdAsync(contrato.Id);
            result.Add(new ContratosYPagosDTO { Contrato = contrato, Pagos = pagos });
        }

        return Ok(result);
    }
}
