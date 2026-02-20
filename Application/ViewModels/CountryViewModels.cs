using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class CountryViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del país es requerido")]
        [Display(Name = "Nombre del país")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código ISO es requerido")]
        [Display(Name = "Código ISO")]
        public string IsoCode { get; set; } = string.Empty;

        // Mantengo estos campos por si los necesitas para el ranking después
        public double Scoring { get; set; }
        public string ReturnRate { get; set; } = string.Empty;
    }
}