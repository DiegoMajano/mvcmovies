using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Data;
using MVCPeliculas.Models;

namespace MVCPeliculas.Controllers;

public class ProyectoController : Controller
{
    private readonly PeliculasDbContext _context;

    public ProyectoController(PeliculasDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Proyectos.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var proyecto = await _context.Proyectos.FirstOrDefaultAsync(m => m.Id == id);
        if (proyecto == null) return NotFound();
        return View(proyecto);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,FechaInicio")] Proyecto proyecto)
    {
        if (ModelState.IsValid)
        {
            _context.Add(proyecto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(proyecto);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto == null) return NotFound();
        return View(proyecto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,FechaInicio")] Proyecto proyecto)
    {
        if (id != proyecto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(proyecto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProyectoExists(proyecto.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(proyecto);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var proyecto = await _context.Proyectos.FirstOrDefaultAsync(m => m.Id == id);
        if (proyecto == null) return NotFound();
        return View(proyecto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto != null) _context.Proyectos.Remove(proyecto);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProyectoExists(int id)
    {
        return _context.Proyectos.Any(e => e.Id == id);
    }
}
