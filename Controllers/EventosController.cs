using DSW_3P_PR02.Models;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventosController(IEventoService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<Evento>>(StatusCodes.Status200OK)]
    public ActionResult<List<Evento>> ObtenerTodos() => Ok(service.ObtenerTodos());

    [HttpGet("{id:int}")]
    [ProducesResponseType<Evento>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public ActionResult<Evento> ObtenerPorId(int id)
    {
        var evento = service.ObtenerPorId(id);
        return evento is null ? NoEncontrado(id) : Ok(evento);
    }

    [HttpPost]
    [ProducesResponseType<Evento>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<Evento> Agregar(Evento nuevo)
    {
        try
        {
            var creado = service.Agregar(nuevo);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
        }
        catch (ArgumentException ex) { return DatosInvalidos(ex.Message); }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult Actualizar(int id, Evento actualizado)
    {
        try { return service.Actualizar(id, actualizado) ? NoContent() : NoEncontrado(id); }
        catch (ArgumentException ex) { return DatosInvalidos(ex.Message); }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id) => service.Eliminar(id) ? NoContent() : NoEncontrado(id);

    private ObjectResult NoEncontrado(int id) =>
        Problem(statusCode: 404, title: "Evento no encontrado", detail: $"No existe el evento con Id {id}.");

    private ObjectResult DatosInvalidos(string mensaje) =>
        Problem(statusCode: 400, title: "Datos inválidos", detail: mensaje);
}
