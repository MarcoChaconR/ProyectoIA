using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoIA.Domain;
using ProyectoIA.Infrastructure;
using ProyectoIA.Web.Models;

namespace ProyectoIA.Web.Controllers;

[Authorize(Roles = "Tecnico,Administrador")]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var esAdmin = User.IsInRole(nameof(Rol.Administrador));
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var gestiones = _db.Gestiones.AsNoTracking();
        if (!esAdmin)
            gestiones = gestiones.Where(g => g.TecnicoAsignadoId == userId);

        var fechasAsignacion = await gestiones
            .Where(g => g.FechaAsignacion.HasValue)
            .Select(g => new { g.Fecha, FechaAsignacion = g.FechaAsignacion!.Value })
            .ToListAsync();
        var cierres = await (
            from bitacora in _db.Bitacora.AsNoTracking()
            join gestion in gestiones on bitacora.GestionId equals gestion.Id
            where bitacora.Accion == "CambioEstado" &&
                  bitacora.ValorNuevo == nameof(EstadoGestion.Cerrada)
            select new { FechaSolicitud = gestion.Fecha, FechaCierre = bitacora.Fecha }
        ).ToListAsync();

        var resumenEstados = await gestiones
            .GroupBy(g => g.Estado)
            .Select(grupo => new { Estado = grupo.Key, Cantidad = grupo.Count() })
            .ToListAsync();

        var total = await gestiones.CountAsync();
        var abiertas = await gestiones.CountAsync(g => g.Estado != EstadoGestion.Cerrada);
        var altaPrioridad = await gestiones.CountAsync(g => g.Prioridad == PrioridadGestion.Alta);

        var model = new DashboardViewModel
        {
            TotalGestiones = total,
            GestionesAbiertas = abiertas,
            GestionesAltaPrioridad = altaPrioridad,
            GestionesSinAsignar = esAdmin
                ? await gestiones.CountAsync(g => g.TecnicoAsignadoId == null)
                : 0,
            HorasPromedioAsignacion = fechasAsignacion.Count == 0
                ? null
                : fechasAsignacion.Average(f => (f.FechaAsignacion - f.Fecha).TotalHours),
            DiasPromedioCierre = cierres.Count == 0
                ? null
                : cierres.Average(f => (f.FechaCierre - f.FechaSolicitud).TotalDays),
            Estados = Enum.GetValues<EstadoGestion>()
                .Select(estado => new DashboardViewModel.EstadoResumen(
                    estado,
                    resumenEstados.FirstOrDefault(resumen => resumen.Estado == estado)?.Cantidad ?? 0))
                .ToList(),
            EsAdministrador = esAdmin
        };

        return View(model);
    }
}
