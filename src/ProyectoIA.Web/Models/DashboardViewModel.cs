using ProyectoIA.Domain;

namespace ProyectoIA.Web.Models;

public class DashboardViewModel
{
    public int TotalGestiones { get; set; }
    public int GestionesAbiertas { get; set; }
    public int GestionesAltaPrioridad { get; set; }
    public int GestionesSinAsignar { get; set; }
    public double? HorasPromedioAsignacion { get; set; }
    public double? DiasPromedioCierre { get; set; }
    public List<EstadoResumen> Estados { get; set; } = new();
    public bool EsAdministrador { get; set; }

    public record EstadoResumen(EstadoGestion Estado, int Cantidad);
}
