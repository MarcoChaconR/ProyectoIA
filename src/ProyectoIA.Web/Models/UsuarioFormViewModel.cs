using System.ComponentModel.DataAnnotations;
using ProyectoIA.Domain;

namespace ProyectoIA.Web.Models;

public class UsuarioFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
    [StringLength(200)]
    [Display(Name = "Correo")]
    public string Correo { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [Display(Name = "Contraseña")]
    public string? Password { get; set; }

    [EnumDataType(typeof(Rol))]
    [Display(Name = "Rol")]
    public Rol Rol { get; set; } = Rol.Cliente;
}
