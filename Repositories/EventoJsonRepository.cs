using System.Text.Json;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public class EventoJsonRepository : IEventoRepository
{
    private readonly string _ruta;
    private readonly object _candado = new();
    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public EventoJsonRepository(IWebHostEnvironment environment, IConfiguration configuration)
    {
        var archivo = configuration["Eventos:Archivo"] ?? "Data/eventos.json";
        _ruta = Path.GetFullPath(archivo, environment.ContentRootPath);
        Directory.CreateDirectory(Path.GetDirectoryName(_ruta)!);
        if (!File.Exists(_ruta)) File.WriteAllText(_ruta, "[]");
    }

    private List<Evento> Leer() =>
        JsonSerializer.Deserialize<List<Evento>>(File.ReadAllText(_ruta), _opciones)
        ?? throw new JsonException("El archivo de eventos debe contener un arreglo JSON.");

    private void Guardar(List<Evento> eventos)
    {
        // Escribir primero un temporal evita dejar el JSON incompleto.
        var temporal = _ruta + ".tmp";
        File.WriteAllText(temporal, JsonSerializer.Serialize(eventos, _opciones));
        File.Move(temporal, _ruta, overwrite: true);
    }

    public List<Evento> ObtenerTodos()
    {
        lock (_candado) return Leer();
    }

    public Evento? ObtenerPorId(int id)
    {
        lock (_candado) return Leer().FirstOrDefault(e => e.Id == id);
    }

    public Evento Agregar(Evento nuevo)
    {
        lock (_candado)
        {
            var eventos = Leer();
            nuevo.Id = eventos.Count == 0 ? 1 : checked(eventos.Max(e => e.Id) + 1);
            eventos.Add(nuevo);
            Guardar(eventos);
            return nuevo;
        }
    }

    public bool Actualizar(int id, Evento actualizado)
    {
        lock (_candado)
        {
            var eventos = Leer();
            var indice = eventos.FindIndex(e => e.Id == id);
            if (indice < 0) return false;
            actualizado.Id = id;
            eventos[indice] = actualizado;
            Guardar(eventos);
            return true;
        }
    }

    public bool Eliminar(int id)
    {
        lock (_candado)
        {
            var eventos = Leer();
            if (eventos.RemoveAll(e => e.Id == id) == 0) return false;
            Guardar(eventos);
            return true;
        }
    }
}
