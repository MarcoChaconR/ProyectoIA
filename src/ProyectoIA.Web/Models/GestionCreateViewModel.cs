using System.ComponentModel.DataAnnotations;

namespace ProyectoIA.Web.Models;

public class GestionCreateViewModel
{
    [Required(ErrorMessage = "El centro de costo es obligatorio.")]
    [Display(Name = "Centro de costo")]
    public int CecoId { get; set; }

    [Required(ErrorMessage = "La dependencia es obligatoria.")]
    [Display(Name = "Dependencia")]
    public int DependenciaId { get; set; }

    [Required(ErrorMessage = "El tipo de solicitud es obligatorio.")]
    [Display(Name = "Tipo de solicitud")]
    public int TipoSolicitudId { get; set; }

    [Required(ErrorMessage = "El objetivo es obligatorio.")]
    [StringLength(200)]
    [Display(Name = "Objetivo de la solicitud")]
    public string Objetivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El detalle es obligatorio.")]
    [Display(Name = "Detalle de la solicitud")]
    public string Detalle { get; set; } = string.Empty;

    [Display(Name = "Referencia de ingreso")]
    public string? ReferenciaIngreso { get; set; }
}
