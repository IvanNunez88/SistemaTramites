using Microsoft.AspNetCore.Mvc;

namespace API.WEB.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperacionesController : ControllerBase
{

    [HttpGet("Sumar/{Numero1:int}/{Numero2:int}")]
    public IActionResult Sumar(int Numero1, int Numero2)
    {
        return Ok($"La suma {Numero1} y {Numero2} es: {Numero1 + Numero2}");
    }

    [HttpPost("ColeccionDatos")]
    public IActionResult ColeccionDatos([FromBody] List<int> lstDatos)
    {
        return Ok(new { Vocales = lstDatos, Numeros = lstDatos });
    }

}
