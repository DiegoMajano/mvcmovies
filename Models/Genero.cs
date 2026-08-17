using System.ComponentModel.DataAnnotations;

namespace MVCPeliculas.Models;

public class Genero
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }

    public ICollection<Pelicula>? Peliculas { get; set; }
}
