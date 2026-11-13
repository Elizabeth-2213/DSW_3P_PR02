using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

public class EventoService(IEventoRepository repository) : IEventoService
{
    public List<Evento> ObtenerTodos() => repository.ObtenerTodos();
    public Evento? ObtenerPorId(int id) => repository.ObtenerPorId(id);

    private static void Validar(Evento evento)
    {
        if (string.IsNullOrWhiteSpace(evento.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");
        if (evento.CupoMaximo <= 0)
            throw new ArgumentException("El cupo máximo debe ser mayor que cero.");
        if (evento.Fecha == default)
            throw new ArgumentException("La fecha es obligatoria y debe ser válida (AAAA-MM-DD).");
        evento.Nombre = evento.Nombre.Trim();
    }

    public Evento Agregar(Evento nuevo)
    {
        Validar(nuevo);
        return repository.Agregar(nuevo);
    }

    public bool Actualizar(int id, Evento actualizado)
    {
        if (repository.ObtenerPorId(id) is null) return false;
        Validar(actualizado);
        return repository.Actualizar(id, actualizado);
    }

    public bool Eliminar(int id) => repository.Eliminar(id);
}
