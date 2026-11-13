using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

public class Evento
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    // DateOnly rechaza fechas imposibles y cadenas con formato incorrecto.
    public DateOnly Fecha { get; set; }

    public string Lugar { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor que cero.")]
    public int CupoMaximo { get; set; }

    public bool Activo { get; set; } = true;
}
