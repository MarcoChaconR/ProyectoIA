using ProyectoIA.Domain;

namespace ProyectoIA.Web.Models;

public class GestionListViewModel
{
    public List<Gestion> Gestiones { get; set; } = new();
    public EstadoGestion? EstadoFiltro { get; set; }
}
