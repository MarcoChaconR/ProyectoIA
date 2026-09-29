using System.ComponentModel.DataAnnotations;

namespace ProyectoIA.Web.Models;

public class TipoSolicitudFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;
}
