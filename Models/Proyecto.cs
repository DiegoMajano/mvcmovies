using System.ComponentModel.DataAnnotations;

namespace MVCPeliculas.Models;

public class Proyecto
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    [Display(Name = "Nombre del Proyecto")]
    public string Nombre { get; set; }

    [Required]
    [StringLength(500)]
    public string Descripcion { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Inicio")]
    public DateTime FechaInicio { get; set; }

    public ICollection<Asignacion>? Asignaciones { get; set; }
}
