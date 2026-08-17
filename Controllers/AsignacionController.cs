using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Data;
using MVCPeliculas.Models;

namespace MVCPeliculas.Controllers;

public class AsignacionController : Controller
{
    private readonly PeliculasDbContext _context;

    public AsignacionController(PeliculasDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var asignaciones = _context.Asignaciones
            .Include(a => a.Empleado)
            .Include(a => a.Proyecto);
        return View(await asignaciones.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var asignacion = await _context.Asignaciones
            .Include(a => a.Empleado)
            .Include(a => a.Proyecto)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (asignacion == null) return NotFound();

        return View(asignacion);
    }

    public IActionResult Create()
    {
        ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre");
        ViewData["ProyectoId"] = new SelectList(_context.Proyectos, "Id", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FechaAsignacion,Rol,EmpleadoId,ProyectoId")] Asignacion asignacion)
    {
        if (ModelState.IsValid)
        {
            _context.Add(asignacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre", asignacion.EmpleadoId);
        ViewData["ProyectoId"] = new SelectList(_context.Proyectos, "Id", "Nombre", asignacion.ProyectoId);
        return View(asignacion);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var asignacion = await _context.Asignaciones.FindAsync(id);
        if (asignacion == null) return NotFound();

        ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre", asignacion.EmpleadoId);
        ViewData["ProyectoId"] = new SelectList(_context.Proyectos, "Id", "Nombre", asignacion.ProyectoId);
        return View(asignacion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FechaAsignacion,Rol,EmpleadoId,ProyectoId")] Asignacion asignacion)
    {
        if (id != asignacion.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(asignacion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AsignacionExists(asignacion.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre", asignacion.EmpleadoId);
        ViewData["ProyectoId"] = new SelectList(_context.Proyectos, "Id", "Nombre", asignacion.ProyectoId);
        return View(asignacion);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var asignacion = await _context.Asignaciones
            .Include(a => a.Empleado)
            .Include(a => a.Proyecto)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (asignacion == null) return NotFound();

        return View(asignacion);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var asignacion = await _context.Asignaciones.FindAsync(id);
        if (asignacion != null) _context.Asignaciones.Remove(asignacion);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AsignacionExists(int id)
    {
        return _context.Asignaciones.Any(e => e.Id == id);
    }
}
