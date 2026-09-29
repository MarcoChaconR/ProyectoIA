using System.ComponentModel.DataAnnotations;

namespace ProyectoIA.Web.Models;

public class GestionCreateViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un centro de costo.")]
    [Display(Name = "Centro de costo")]
    public int CecoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una dependencia.")]
    [Display(Name = "Dependencia")]
    public int DependenciaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de solicitud.")]
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
