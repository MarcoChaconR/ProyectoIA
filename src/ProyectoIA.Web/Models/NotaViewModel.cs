using System.ComponentModel.DataAnnotations;

namespace ProyectoIA.Web.Models;

public class NotaViewModel
{
    [Required(ErrorMessage = "El texto de la nota es obligatorio.")]
    [Display(Name = "Nota")]
    public string Texto { get; set; } = string.Empty;

    [Display(Name = "Nota pública (visible para el cliente)")]
    public bool EsPublica { get; set; } = true;
}
