using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class MacroindicadorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El peso es obligatorio")]
        [Range(0, 1, ErrorMessage = "El peso debe estar entre 0 y 1")]
        public decimal Peso { get; set; }

        [Display(Name = "Es mejor más alto")]
        public bool MasAltoMejor { get; set; }
    }
}