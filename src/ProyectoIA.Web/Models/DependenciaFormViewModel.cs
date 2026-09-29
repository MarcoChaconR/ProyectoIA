using System.ComponentModel.DataAnnotations;

namespace ProyectoIA.Web.Models;

public class DependenciaFormViewModel
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un CECO.")]
    [Display(Name = "CECO")]
    public int CecoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;
}
