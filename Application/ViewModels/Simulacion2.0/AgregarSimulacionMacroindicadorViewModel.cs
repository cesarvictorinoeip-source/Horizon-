using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulacion2._0
{
    public class AgregarSimulacionMacroindicadorViewModel
    {
        [Required(ErrorMessage = "Debes seleccionar un macroindicador")]
        public int? MacroindicadorId { get; set; }

        [Required(ErrorMessage = "Debes ingresar el peso de simulación")]
        [Range(0.0001, 1.0000, ErrorMessage = "El peso debe estar entre 0.0001 y 1.0000")]
        public decimal? PesoSimulacion { get; set; }

        // Para el select
        public List<SelectListItem> MacroindicadoresDisponibles { get; set; } = new();

    }
}
