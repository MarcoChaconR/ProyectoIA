using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoIA.Domain;
using ProyectoIA.Infrastructure;
using ProyectoIA.Web.Models;

namespace ProyectoIA.Web.Controllers;

[Authorize]
public class GestionesController : Controller
{
    private readonly AppDbContext _db;

    public GestionesController(AppDbContext db) => _db = db;

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> Index(EstadoGestion? estado = null)
    {
        var query = _db.Gestiones
            .Include(g => g.Solicitante)
            .Include(g => g.TecnicoAsignado)
            .Include(g => g.Ceco)
            .Include(g => g.Dependencia)
            .Include(g => g.TipoSolicitud)
            .AsQueryable();

        if (User.IsInRole(nameof(Rol.Cliente)))
            query = query.Where(g => g.SolicitanteId == CurrentUserId);
        else if (User.IsInRole(nameof(Rol.Tecnico)))
            query = query.Where(g => g.TecnicoAsignadoId == CurrentUserId);

        if (estado.HasValue)
            query = query.Where(g => g.Estado == estado.Value);

        var model = new GestionListViewModel
        {
            Gestiones = await query.OrderByDescending(g => g.Fecha).ToListAsync(),
            EstadoFiltro = estado
        };

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = nameof(Rol.Cliente))]
    public async Task<IActionResult> Create()
    {
        await CargarCatalogos();
        return View(new GestionCreateViewModel());
    }

    [HttpPost]
    [Authorize(Roles = nameof(Rol.Cliente))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GestionCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await CargarCatalogos();
            return View(model);
        }

        var gestion = new Gestion
        {
            Fecha = DateTime.Now,
            CecoId = model.CecoId,
            DependenciaId = model.DependenciaId,
            TipoSolicitudId = model.TipoSolicitudId,
            Objetivo = model.Objetivo,
            Detalle = model.Detalle,
            ReferenciaIngreso = model.ReferenciaIngreso,
            Estado = EstadoGestion.Registrada,
            FechaCambioEstado = DateTime.Now,
            SolicitanteId = CurrentUserId
        };

        _db.Gestiones.Add(gestion);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = gestion.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var gestion = await _db.Gestiones
            .Include(g => g.Solicitante)
            .Include(g => g.TecnicoAsignado)
            .Include(g => g.Ceco)
            .Include(g => g.Dependencia)
            .Include(g => g.TipoSolicitud)
            .Include(g => g.Notas)
                .ThenInclude(n => n.Autor)
            .Include(g => g.Bitacora)
                .ThenInclude(b => b.Autor)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gestion is null)
            return NotFound();

        if (!PuedeVerGestion(gestion))
            return Forbid();

        var esCliente = User.IsInRole(nameof(Rol.Cliente));

        ViewData["EsCliente"] = esCliente;
        ViewData["PuedeAvanzarEstado"] = !esCliente && gestion.Estado != EstadoGestion.Cerrada;
        ViewData["EsAdmin"] = User.IsInRole(nameof(Rol.Administrador));

        if (User.IsInRole(nameof(Rol.Administrador)))
        {
            ViewData["Tecnicos"] = new SelectList(
                await _db.Usuarios.Where(u => u.Rol == Rol.Tecnico).ToListAsync(),
                nameof(Usuario.Id),
                nameof(Usuario.Nombre),
                gestion.TecnicoAsignadoId);
        }

        return View(gestion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarNota(int id, NotaViewModel model)
    {
        var gestion = await _db.Gestiones.FindAsync(id);
        if (gestion is null)
            return NotFound();

        if (!PuedeVerGestion(gestion))
            return Forbid();

        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Details), new { id });

        var esCliente = User.IsInRole(nameof(Rol.Cliente));

        var nota = new NotaGestion
        {
            GestionId = id,
            Texto = model.Texto,
            EsPublica = esCliente || model.EsPublica,
            AutorId = CurrentUserId,
            Fecha = DateTime.Now
        };

        _db.Notas.Add(nota);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize(Roles = "Tecnico,Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int id)
    {
        var gestion = await _db.Gestiones.FindAsync(id);
        if (gestion is null)
            return NotFound();

        if (gestion.Estado == EstadoGestion.Cerrada)
            return RedirectToAction(nameof(Details), new { id });

        var anterior = gestion.Estado;
        var siguiente = (EstadoGestion)((int)gestion.Estado + 1);

        gestion.Estado = siguiente;
        gestion.FechaCambioEstado = DateTime.Now;

        _db.Bitacora.Add(new BitacoraCambio
        {
            GestionId = id,
            Accion = "CambioEstado",
            ValorAnterior = anterior.ToString(),
            ValorNuevo = siguiente.ToString(),
            AutorId = CurrentUserId,
            Fecha = DateTime.Now
        });

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize(Roles = nameof(Rol.Administrador))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarTecnico(int id, int tecnicoId)
    {
        var gestion = await _db.Gestiones.FindAsync(id);
        if (gestion is null)
            return NotFound();

        var tecnico = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.Id == tecnicoId && u.Rol == Rol.Tecnico);
        if (tecnico is null)
            return RedirectToAction(nameof(Details), new { id });

        gestion.TecnicoAsignadoId = tecnico.Id;
        gestion.FechaAsignacion = DateTime.Now;

        _db.Bitacora.Add(new BitacoraCambio
        {
            GestionId = id,
            Accion = "Asignacion",
            ValorAnterior = null,
            ValorNuevo = tecnico.Nombre,
            AutorId = CurrentUserId,
            Fecha = DateTime.Now
        });

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id });
    }

    private bool PuedeVerGestion(Gestion gestion)
    {
        if (User.IsInRole(nameof(Rol.Administrador)))
            return true;

        if (User.IsInRole(nameof(Rol.Tecnico)))
            return gestion.TecnicoAsignadoId == CurrentUserId;

        return gestion.SolicitanteId == CurrentUserId;
    }

    private async Task CargarCatalogos()
    {
        ViewData["Cecos"] = new SelectList(await _db.Cecos.OrderBy(c => c.Codigo).ToListAsync(), nameof(Ceco.Id), nameof(Ceco.Nombre));
        ViewData["Dependencias"] = new SelectList(await _db.Dependencias.OrderBy(d => d.Nombre).ToListAsync(), nameof(Dependencia.Id), nameof(Dependencia.Nombre));
        ViewData["TiposSolicitud"] = new SelectList(await _db.TiposSolicitud.OrderBy(t => t.Nombre).ToListAsync(), nameof(TipoSolicitud.Id), nameof(TipoSolicitud.Nombre));
    }
}
