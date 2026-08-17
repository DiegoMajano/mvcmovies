using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Data;
using MVCPeliculas.Models;

namespace MVCPeliculas.Controllers;

public class PeliculaController : Controller
{
    private readonly PeliculasDbContext _context;

    public PeliculaController(PeliculasDbContext context)
    {
        _context = context;
    }

    // GET: Pelicula
    public async Task<IActionResult> Index()
    {
        var peliculas = _context.Peliculas.Include(p => p.Genero);
        return View(await peliculas.ToListAsync());
    }

    // GET: Pelicula/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var pelicula = await _context.Peliculas
            .Include(p => p.Genero)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pelicula == null) return NotFound();

        return View(pelicula);
    }

    // GET: Pelicula/Create
    public IActionResult Create()
    {
        ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre");
        return View();
    }

    // POST: Pelicula/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Titulo,FechaLanzamiento,Precio,Director,GeneroId")] Pelicula pelicula)
    {
        if (ModelState.IsValid)
        {
            _context.Add(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
        return View(pelicula);
    }

    // GET: Pelicula/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var pelicula = await _context.Peliculas.FindAsync(id);
        if (pelicula == null) return NotFound();

        ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
        return View(pelicula);
    }

    // POST: Pelicula/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,FechaLanzamiento,Precio,Director,GeneroId")] Pelicula pelicula)
    {
        if (id != pelicula.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pelicula);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeliculaExists(pelicula.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
        return View(pelicula);
    }

    // GET: Pelicula/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var pelicula = await _context.Peliculas
            .Include(p => p.Genero)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pelicula == null) return NotFound();

        return View(pelicula);
    }

    // POST: Pelicula/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pelicula = await _context.Peliculas.FindAsync(id);
        if (pelicula != null) _context.Peliculas.Remove(pelicula);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PeliculaExists(int id)
    {
        return _context.Peliculas.Any(e => e.Id == id);
    }
}
