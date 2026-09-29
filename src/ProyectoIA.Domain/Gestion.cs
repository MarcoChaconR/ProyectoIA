namespace ProyectoIA.Domain;

public class Gestion
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }

    public int CecoId { get; set; }
    public Ceco? Ceco { get; set; }

    public int DependenciaId { get; set; }
    public Dependencia? Dependencia { get; set; }

    public int TipoSolicitudId { get; set; }
    public TipoSolicitud? TipoSolicitud { get; set; }

    public string Objetivo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public string? ReferenciaIngreso { get; set; }

    public EstadoGestion Estado { get; set; } = EstadoGestion.Registrada;
    public DateTime FechaCambioEstado { get; set; }

    public int SolicitanteId { get; set; }
    public Usuario? Solicitante { get; set; }

    public int? TecnicoAsignadoId { get; set; }
    public Usuario? TecnicoAsignado { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public ICollection<NotaGestion> Notas { get; set; } = new List<NotaGestion>();
    public ICollection<BitacoraCambio> Bitacora { get; set; } = new List<BitacoraCambio>();
}
