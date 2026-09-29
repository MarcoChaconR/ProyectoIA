using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoIA.Domain;
using ProyectoIA.Infrastructure;
using ProyectoIA.Web.Models;

namespace ProyectoIA.Web.Controllers;

[Authorize(Roles = nameof(Rol.Administrador))]
public class CatalogosController : Controller
{
    private readonly AppDbContext _db;

    public CatalogosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.Cecos = await _db.Cecos.OrderBy(c => c.Codigo).ToListAsync();
        ViewBag.Dependencias = await _db.Dependencias
            .Include(d => d.Ceco)
            .OrderBy(d => d.Ceco!.Codigo)
            .ThenBy(d => d.Nombre)
            .ToListAsync();
        ViewBag.TiposSolicitud = await _db.TiposSolicitud.OrderBy(t => t.Nombre).ToListAsync();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Ceco(int? id)
    {
        if (id is null)
            return View("CecoForm", new CecoFormViewModel());

        var ceco = await _db.Cecos.FindAsync(id);
        if (ceco is null)
            return NotFound();

        return View("CecoForm", new CecoFormViewModel
        {
            Id = ceco.Id,
            Codigo = ceco.Codigo,
            Nombre = ceco.Nombre
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarCeco(CecoFormViewModel model)
    {
        model.Codigo = model.Codigo.Trim();
        model.Nombre = model.Nombre.Trim();
        var existeCodigo = await _db.Cecos.AnyAsync(c =>
            c.Codigo == model.Codigo && c.Id != model.Id);
        if (existeCodigo)
            ModelState.AddModelError(nameof(model.Codigo), "Ya existe un CECO con ese código.");

        if (!ModelState.IsValid)
            return View("CecoForm", model);

        Ceco ceco;
        if (model.Id == 0)
        {
            ceco = new Ceco();
            _db.Cecos.Add(ceco);
        }
        else
        {
            ceco = await _db.Cecos.FindAsync(model.Id) ?? throw new InvalidOperationException("El CECO ya no existe.");
        }

        ceco.Codigo = model.Codigo;
        ceco.Nombre = model.Nombre;
        ceco.Activo = true;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DesactivarCeco(int id)
    {
        var ceco = await _db.Cecos.FindAsync(id);
        if (ceco is null)
            return NotFound();

        if (ceco.Activo && await _db.Cecos.CountAsync(c => c.Activo) <= 1)
            return BadRequest("Debe quedar al menos un CECO activo.");

        ceco.Activo = false;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Dependencia(int? id)
    {
        await CargarCecosActivos();
        if (id is null)
            return View("DependenciaForm", new DependenciaFormViewModel());

        var dependencia = await _db.Dependencias.FindAsync(id);
        if (dependencia is null)
            return NotFound();

        return View("DependenciaForm", new DependenciaFormViewModel
        {
            Id = dependencia.Id,
            CecoId = dependencia.CecoId,
            Nombre = dependencia.Nombre
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarDependencia(DependenciaFormViewModel model)
    {
        model.Nombre = model.Nombre.Trim();
        var cecoActivo = await _db.Cecos.AnyAsync(c => c.Id == model.CecoId && c.Activo);
        if (!cecoActivo)
            ModelState.AddModelError(nameof(model.CecoId), "Seleccione un CECO activo.");

        var existeNombre = await _db.Dependencias.AnyAsync(d =>
            d.CecoId == model.CecoId && d.Nombre == model.Nombre && d.Id != model.Id);
        if (existeNombre)
            ModelState.AddModelError(nameof(model.Nombre), "Ya existe una dependencia con ese nombre para el CECO.");

        if (!ModelState.IsValid)
        {
            await CargarCecosActivos();
            return View("DependenciaForm", model);
        }

        Dependencia dependencia;
        if (model.Id == 0)
        {
            dependencia = new Dependencia();
            _db.Dependencias.Add(dependencia);
        }
        else
        {
            dependencia = await _db.Dependencias.FindAsync(model.Id)
                ?? throw new InvalidOperationException("La dependencia ya no existe.");
        }

        dependencia.CecoId = model.CecoId;
        dependencia.Nombre = model.Nombre;
        dependencia.Activa = true;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DesactivarDependencia(int id)
    {
        var dependencia = await _db.Dependencias.FindAsync(id);
        if (dependencia is null)
            return NotFound();

        if (dependencia.Activa &&
            await _db.Dependencias.CountAsync(d => d.CecoId == dependencia.CecoId && d.Activa) <= 1)
            return BadRequest("Debe quedar al menos una dependencia activa para ese CECO.");

        dependencia.Activa = false;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> TipoSolicitud(int? id)
    {
        if (id is null)
            return View("TipoSolicitudForm", new TipoSolicitudFormViewModel());

        var tipo = await _db.TiposSolicitud.FindAsync(id);
        if (tipo is null)
            return NotFound();

        return View("TipoSolicitudForm", new TipoSolicitudFormViewModel
        {
            Id = tipo.Id,
            Nombre = tipo.Nombre
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarTipoSolicitud(TipoSolicitudFormViewModel model)
    {
        model.Nombre = model.Nombre.Trim();
        var existeNombre = await _db.TiposSolicitud.AnyAsync(t =>
            t.Nombre == model.Nombre && t.Id != model.Id);
        if (existeNombre)
            ModelState.AddModelError(nameof(model.Nombre), "Ya existe un tipo con ese nombre.");

        if (!ModelState.IsValid)
            return View("TipoSolicitudForm", model);

        TipoSolicitud tipo;
        if (model.Id == 0)
        {
            tipo = new TipoSolicitud();
            _db.TiposSolicitud.Add(tipo);
        }
        else
        {
            tipo = await _db.TiposSolicitud.FindAsync(model.Id)
                ?? throw new InvalidOperationException("El tipo de solicitud ya no existe.");
        }

        tipo.Nombre = model.Nombre;
        tipo.Activo = true;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DesactivarTipoSolicitud(int id)
    {
        var tipo = await _db.TiposSolicitud.FindAsync(id);
        if (tipo is null)
            return NotFound();

        if (tipo.Activo && await _db.TiposSolicitud.CountAsync(t => t.Activo) <= 1)
            return BadRequest("Debe quedar al menos un tipo de solicitud activo.");

        tipo.Activo = false;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCecosActivos()
    {
        ViewBag.Cecos = new SelectList(
            await _db.Cecos.Where(c => c.Activo).OrderBy(c => c.Codigo).ToListAsync(),
            nameof(ProyectoIA.Domain.Ceco.Id),
            nameof(ProyectoIA.Domain.Ceco.Codigo));
    }
}
