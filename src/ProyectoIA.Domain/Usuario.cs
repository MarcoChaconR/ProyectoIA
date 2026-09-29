namespace ProyectoIA.Domain;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Rol Rol { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Gestion> GestionesSolicitadas { get; set; } = new List<Gestion>();
    public ICollection<Gestion> GestionesAsignadas { get; set; } = new List<Gestion>();
}
