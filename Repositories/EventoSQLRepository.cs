using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public class EventoSQLRepository : IEventoRepository
{
    private readonly AppDbContext _context;

    public EventoSQLRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Evento> ObtenerTodos()
    {
        return _context.Eventos.OrderBy(e => e.Id).ToList();
    }

    public Evento? ObtenerPorId(int id)
    {
        return _context.Eventos.Find(id);
    }

    public Evento Agregar(Evento nuevo)
    {
        nuevo.Id = 0;

        _context.Eventos.Add(nuevo);
        _context.SaveChanges();

        return nuevo;
    }

    public bool Actualizar(int id, Evento actualizado)
    {
        var evento = _context.Eventos.Find(id);

        if (evento is null)
            return false;

        evento.Nombre = actualizado.Nombre;
        evento.Categoria = actualizado.Categoria;
        evento.Fecha = actualizado.Fecha;
        evento.Lugar = actualizado.Lugar;
        evento.CupoMaximo = actualizado.CupoMaximo;
        evento.Activo = actualizado.Activo;

        _context.SaveChanges();

        return true;
    }

    public bool Eliminar(int id)
    {
        var evento = _context.Eventos.Find(id);

        if (evento is null)
            return false;

        _context.Eventos.Remove(evento);
        _context.SaveChanges();

        return true;
    }
}