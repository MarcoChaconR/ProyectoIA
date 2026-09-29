using System.ComponentModel.DataAnnotations;

namespace ProyectoIA.Web.Models;

public class CecoFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El código es obligatorio.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El código debe contener solo números.")]
    [StringLength(20)]
    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;
}
