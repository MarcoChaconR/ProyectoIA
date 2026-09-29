namespace ProyectoIA.Domain;

public class NotaGestion
{
    public int Id { get; set; }

    public int GestionId { get; set; }
    public Gestion? Gestion { get; set; }

    public string Texto { get; set; } = string.Empty;
    public bool EsPublica { get; set; }

    public int AutorId { get; set; }
    public Usuario? Autor { get; set; }

    public DateTime Fecha { get; set; }
}
