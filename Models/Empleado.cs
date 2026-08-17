using System.ComponentModel.DataAnnotations;

namespace MVCPeliculas.Models;

public class Empleado
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Contratación")]
    public DateTime FechaContratacion { get; set; }

    [Required]
    [StringLength(50)]
    public string Puesto { get; set; }

    public ICollection<Asignacion>? Asignaciones { get; set; }
}
