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
    public async Task<IActionResult> Index(
        EstadoGestion? estado = null,
        int? cecoId = null,
        int? dependenciaId = null,
        int? tipoSolicitudId = null,
        int? tecnicoId = null,
        PrioridadGestion? prioridad = null)
    {
        if (estado.HasValue && !Enum.IsDefined(estado.Value) ||
            prioridad.HasValue && !Enum.IsDefined(prioridad.Value))
            return BadRequest();

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
        if (cecoId.HasValue)
            query = query.Where(g => g.CecoId == cecoId.Value);
        if (dependenciaId.HasValue)
            query = query.Where(g => g.DependenciaId == dependenciaId.Value);
        if (tipoSolicitudId.HasValue)
            query = query.Where(g => g.TipoSolicitudId == tipoSolicitudId.Value);
        if (prioridad.HasValue)
            query = query.Where(g => g.Prioridad == prioridad.Value);
        if (tecnicoId.HasValue)
            query = query.Where(g => g.TecnicoAsignadoId == tecnicoId.Value);

        var model = new GestionListViewModel
        {
            Gestiones = await query.OrderByDescending(g => g.Fecha).ToListAsync(),
            EstadoFiltro = estado,
            CecoFiltro = cecoId,
            DependenciaFiltro = dependenciaId,
            TipoSolicitudFiltro = tipoSolicitudId,
            TecnicoFiltro = tecnicoId,
            PrioridadFiltro = prioridad,
            Cecos = await _db.Cecos.Where(c => c.Activo).OrderBy(c => c.Codigo).ToListAsync(),
            Dependencias = await _db.Dependencias
                .Where(d => d.Activa && d.Ceco!.Activo)
                .OrderBy(d => d.Nombre)
                .ToListAsync(),
            TiposSolicitud = await _db.TiposSolicitud.Where(t => t.Activo).OrderBy(t => t.Nombre).ToListAsync(),
            Tecnicos = await _db.Usuarios
                .Where(u => u.Activo && u.Rol == Rol.Tecnico)
                .OrderBy(u => u.Nombre)
                .ToListAsync()
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
        if (!await _db.Cecos.AnyAsync(c => c.Id == model.CecoId && c.Activo))
            ModelState.AddModelError(nameof(model.CecoId), "Seleccione un CECO activo.");

        if (!await _db.Dependencias.AnyAsync(d =>
                d.Id == model.DependenciaId && d.CecoId == model.CecoId && d.Activa))
            ModelState.AddModelError(nameof(model.DependenciaId), "Seleccione una dependencia activa del CECO indicado.");

        if (!await _db.TiposSolicitud.AnyAsync(t => t.Id == model.TipoSolicitudId && t.Activo))
            ModelState.AddModelError(nameof(model.TipoSolicitudId), "Seleccione un tipo de solicitud activo.");

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
        ViewData["PuedeEditar"] = User.IsInRole(nameof(Rol.Administrador)) ||
            User.IsInRole(nameof(Rol.Tecnico)) && gestion.TecnicoAsignadoId == CurrentUserId;

        if (User.IsInRole(nameof(Rol.Administrador)))
        {
            ViewData["Tecnicos"] = new SelectList(
                await _db.Usuarios.Where(u => u.Rol == Rol.Tecnico && u.Activo).ToListAsync(),
                nameof(Usuario.Id),
                nameof(Usuario.Nombre),
                gestion.TecnicoAsignadoId);
        }

        return View(gestion);
    }

    [HttpGet]
    [Authorize(Roles = "Tecnico,Administrador")]
    public async Task<IActionResult> Edit(int id)
    {
        var gestion = await _db.Gestiones.FindAsync(id);
        if (gestion is null)
            return NotFound();
        if (!PuedeEditarGestion(gestion))
            return Forbid();

        var model = new GestionEditViewModel
        {
            Id = gestion.Id,
            Version = gestion.Version,
            CecoId = gestion.CecoId,
            DependenciaId = gestion.DependenciaId,
            TipoSolicitudId = gestion.TipoSolicitudId,
            Objetivo = gestion.Objetivo,
            Detalle = gestion.Detalle,
            ReferenciaIngreso = gestion.ReferenciaIngreso,
            Prioridad = gestion.Prioridad
        };
        await CargarCatalogosEdicion(model);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Tecnico,Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GestionEditViewModel model)
    {
        if (id != model.Id)
            return BadRequest();

        var gestion = await _db.Gestiones.FindAsync(id);
        if (gestion is null)
            return NotFound();
        if (!PuedeEditarGestion(gestion))
            return Forbid();
        _db.Entry(gestion).Property(g => g.Version).OriginalValue = model.Version;

        if (!await _db.Cecos.AnyAsync(c =>
                c.Id == model.CecoId && (c.Activo || c.Id == gestion.CecoId)))
            ModelState.AddModelError(nameof(model.CecoId), "Seleccione un CECO activo.");
        if (!await _db.Dependencias.AnyAsync(d =>
                d.Id == model.DependenciaId && d.CecoId == model.CecoId &&
                (d.Activa || d.Id == gestion.DependenciaId && model.CecoId == gestion.CecoId)))
            ModelState.AddModelError(nameof(model.DependenciaId), "Seleccione una dependencia activa del CECO indicado.");
        if (!await _db.TiposSolicitud.AnyAsync(t =>
                t.Id == model.TipoSolicitudId && (t.Activo || t.Id == gestion.TipoSolicitudId)))
            ModelState.AddModelError(nameof(model.TipoSolicitudId), "Seleccione un tipo de solicitud activo.");

        if (!ModelState.IsValid)
        {
            await CargarCatalogosEdicion(model);
            return View(model);
        }

        var cambios = new List<(string Campo, string? Anterior, string? Nuevo)>();
        RegistrarCambio(cambios, "CecoId", gestion.CecoId, model.CecoId);
        RegistrarCambio(cambios, "DependenciaId", gestion.DependenciaId, model.DependenciaId);
        RegistrarCambio(cambios, "TipoSolicitudId", gestion.TipoSolicitudId, model.TipoSolicitudId);
        RegistrarCambio(cambios, "Objetivo", gestion.Objetivo, model.Objetivo.Trim());
        RegistrarCambio(cambios, "Detalle", gestion.Detalle, model.Detalle.Trim());
        RegistrarCambio(cambios, "ReferenciaIngreso", gestion.ReferenciaIngreso, model.ReferenciaIngreso?.Trim());
        RegistrarCambio(cambios, "Prioridad", gestion.Prioridad, model.Prioridad);

        gestion.CecoId = model.CecoId;
        gestion.DependenciaId = model.DependenciaId;
        gestion.TipoSolicitudId = model.TipoSolicitudId;
        gestion.Objetivo = model.Objetivo.Trim();
        gestion.Detalle = model.Detalle.Trim();
        gestion.ReferenciaIngreso = string.IsNullOrWhiteSpace(model.ReferenciaIngreso)
            ? null
            : model.ReferenciaIngreso.Trim();
        gestion.Prioridad = model.Prioridad;
        gestion.Version += 1;

        var fecha = DateTime.Now;
        foreach (var cambio in cambios)
        {
            _db.Bitacora.Add(new BitacoraCambio
            {
                GestionId = id,
                Accion = $"Edicion:{cambio.Campo}",
                ValorAnterior = cambio.Anterior,
                ValorNuevo = cambio.Nuevo,
                AutorId = CurrentUserId,
                Fecha = fecha
            });
        }

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError(string.Empty, "La gestión cambió desde que la abriste. Recargá y aplicá tus cambios de nuevo.");
            await CargarCatalogosEdicion(model);
            return View(model);
        }
        return RedirectToAction(nameof(Details), new { id });
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
        if (!PuedeVerGestion(gestion))
            return Forbid();

        if (gestion.Estado == EstadoGestion.Cerrada)
            return RedirectToAction(nameof(Details), new { id });

        var anterior = gestion.Estado;
        var siguiente = (EstadoGestion)((int)gestion.Estado + 1);

        gestion.Estado = siguiente;
        gestion.FechaCambioEstado = DateTime.Now;
        gestion.Version += 1;

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
            .FirstOrDefaultAsync(u => u.Id == tecnicoId && u.Rol == Rol.Tecnico && u.Activo);
        if (tecnico is null)
            return BadRequest("Seleccione un técnico activo.");

        var tecnicoAnterior = gestion.TecnicoAsignadoId.HasValue
            ? await _db.Usuarios.Where(u => u.Id == gestion.TecnicoAsignadoId).Select(u => u.Nombre).FirstOrDefaultAsync()
            : null;
        gestion.TecnicoAsignadoId = tecnico.Id;
        gestion.FechaAsignacion = DateTime.Now;
        gestion.Version += 1;

        _db.Bitacora.Add(new BitacoraCambio
        {
            GestionId = id,
            Accion = "Asignacion",
            ValorAnterior = tecnicoAnterior,
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

    private bool PuedeEditarGestion(Gestion gestion) =>
        User.IsInRole(nameof(Rol.Administrador)) ||
        User.IsInRole(nameof(Rol.Tecnico)) && gestion.TecnicoAsignadoId == CurrentUserId;

    private async Task CargarCatalogosEdicion(GestionEditViewModel model)
    {
        ViewData["Cecos"] = new SelectList(
            await _db.Cecos
                .Where(c => c.Activo || c.Id == model.CecoId)
                .OrderBy(c => c.Codigo)
                .ToListAsync(),
            nameof(ProyectoIA.Domain.Ceco.Id),
            nameof(ProyectoIA.Domain.Ceco.Codigo),
            model.CecoId);
        ViewData["Dependencias"] = await _db.Dependencias
            .Where(d => (d.Activa && d.Ceco!.Activo) || d.Id == model.DependenciaId)
            .OrderBy(d => d.Nombre)
            .ToListAsync();
        ViewData["TiposSolicitud"] = new SelectList(
            await _db.TiposSolicitud
                .Where(t => t.Activo || t.Id == model.TipoSolicitudId)
                .OrderBy(t => t.Nombre)
                .ToListAsync(),
            nameof(TipoSolicitud.Id),
            nameof(TipoSolicitud.Nombre),
            model.TipoSolicitudId);
    }

    private static void RegistrarCambio<T>(
        ICollection<(string Campo, string? Anterior, string? Nuevo)> cambios,
        string campo,
        T anterior,
        T nuevo)
    {
        if (!EqualityComparer<T>.Default.Equals(anterior, nuevo))
            cambios.Add((campo, Convert.ToString(anterior), Convert.ToString(nuevo)));
    }

    private async Task CargarCatalogos()
    {
        ViewData["Cecos"] = new SelectList(
            await _db.Cecos.Where(c => c.Activo).OrderBy(c => c.Codigo).ToListAsync(),
            nameof(Ceco.Id),
            nameof(Ceco.Codigo));
        ViewData["Dependencias"] = await _db.Dependencias
            .Where(d => d.Activa && d.Ceco!.Activo)
            .OrderBy(d => d.Nombre)
            .ToListAsync();
        ViewData["TiposSolicitud"] = new SelectList(
            await _db.TiposSolicitud.Where(t => t.Activo).OrderBy(t => t.Nombre).ToListAsync(),
            nameof(TipoSolicitud.Id),
            nameof(TipoSolicitud.Nombre));
    }
}
