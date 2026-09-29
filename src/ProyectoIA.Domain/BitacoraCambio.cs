namespace ProyectoIA.Domain;

/// <summary>
/// Registro de auditoría para cambios de estado y asignación de técnico.
/// Sin UI dedicada en el MVP, pero se persiste para trazabilidad futura.
/// </summary>
public class BitacoraCambio
{
    public int Id { get; set; }

    public int GestionId { get; set; }
    public Gestion? Gestion { get; set; }

    public string Accion { get; set; } = string.Empty;
    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }

    public int AutorId { get; set; }
    public Usuario? Autor { get; set; }

    public DateTime Fecha { get; set; }
}
