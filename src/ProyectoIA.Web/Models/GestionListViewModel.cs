using ProyectoIA.Domain;

namespace ProyectoIA.Web.Models;

public class GestionListViewModel
{
    public List<Gestion> Gestiones { get; set; } = new();
    public EstadoGestion? EstadoFiltro { get; set; }
    public int? CecoFiltro { get; set; }
    public int? DependenciaFiltro { get; set; }
    public int? TipoSolicitudFiltro { get; set; }
    public int? TecnicoFiltro { get; set; }
    public PrioridadGestion? PrioridadFiltro { get; set; }
    public List<Ceco> Cecos { get; set; } = new();
    public List<Dependencia> Dependencias { get; set; } = new();
    public List<TipoSolicitud> TiposSolicitud { get; set; } = new();
    public List<Usuario> Tecnicos { get; set; } = new();
}
