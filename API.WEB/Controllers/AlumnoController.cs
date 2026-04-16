using Microsoft.AspNetCore.Mvc;
using API.WEB.Features.Alumnos;
using API.WEB.Features.Alumnos.Dominio.Dto;

namespace API.WEB.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlumnoController : ControllerBase
{
    private readonly AlumnoService _service;

    public AlumnoController(AlumnoService service)
    {
        _service = service;
    }

    [HttpGet("ObtenerTodos")]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _service.ObtenerTodos());
    }

    [HttpGet("ObtenerPorMatricula/{matricula:int}")]
    public async Task<IActionResult> ObtenerPorMatricula(int matricula)
    {
        return Ok(await _service.ObtenerPorMatricula(matricula));
    }

    [HttpPost("Crear")]
    public async Task<IActionResult> Crear([FromBody] CrearAlumnoDTO dto)
    {
        return Ok(await _service.Crear(dto));
    }

    [HttpPut("Actualizar/{matricula:int}")]
    public async Task<IActionResult> Actualizar(int matricula, [FromBody] ActualizarAlumnoDTO dto)
    {
        return Ok(await _service.Actualizar(matricula, dto));
    }

    [HttpDelete("Eliminar/{matricula:int}")]
    public async Task<IActionResult> Eliminar(int matricula)
    {
        return Ok(await _service.Eliminar(matricula));
    }
}
