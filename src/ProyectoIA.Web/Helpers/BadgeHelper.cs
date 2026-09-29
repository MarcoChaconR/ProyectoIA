using ProyectoIA.Domain;

namespace ProyectoIA.Web.Helpers;

/// <summary>
/// Centraliza el mapeo entre valores de enum de negocio y clases de badge de Bootstrap,
/// para que el mismo estado/prioridad/rol se vea igual en todas las vistas (Views/_Layout.cshtml
/// y checklist de consistencia).
/// </summary>
public static class BadgeHelper
{
    public static string Clase(EstadoGestion estado) => estado switch
    {
        EstadoGestion.Registrada => "text-bg-secondary",
        EstadoGestion.Asignada => "text-bg-info",
        EstadoGestion.EnAtencion => "text-bg-primary",
        EstadoGestion.Resuelta => "text-bg-success",
        EstadoGestion.Cerrada => "text-bg-dark",
        _ => "text-bg-secondary"
    };

    public static string Clase(PrioridadGestion prioridad) => prioridad switch
    {
        PrioridadGestion.Baja => "text-bg-secondary",
        PrioridadGestion.Media => "text-bg-warning",
        PrioridadGestion.Alta => "text-bg-danger",
        _ => "text-bg-secondary"
    };

    public static string Clase(Rol rol) => rol switch
    {
        Rol.Cliente => "text-bg-secondary",
        Rol.Tecnico => "text-bg-info",
        Rol.Administrador => "text-bg-primary",
        _ => "text-bg-secondary"
    };

    public static string ClaseEstadoActivo(bool activo) => activo ? "text-bg-success" : "text-bg-secondary";
}
