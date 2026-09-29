namespace ProyectoIA.Domain;

public class Dependencia
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int CecoId { get; set; }
    public Ceco? Ceco { get; set; }
    public bool Activa { get; set; } = true;
}
