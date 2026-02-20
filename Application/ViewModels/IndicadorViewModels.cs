using Microsoft.AspNetCore.Mvc.Rendering; // <-- ESTA LÍNEA ES VITAL
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class IndicadorViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El país es requerido")]
        public int PaisId { get; set; }
        public string? PaisNombre { get; set; }

        [Required(ErrorMessage = "El macroindicador es requerido")]
        public int MacroindicadorId { get; set; }
        public string? MacroindicadorNombre { get; set; }

        [Required(ErrorMessage = "El valor es requerido")]
        public decimal Valor { get; set; } // Ahora es decimal

        [Required(ErrorMessage = "El año es requerido")]
        public int Anio { get; set; }

        public List<SelectListItem>? Paises { get; set; }
        public List<SelectListItem>? Macroindicadores { get; set; }
    }
}