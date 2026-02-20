using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulacion2._0
{
    public class EditarSimulacionMacroindicadorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes ingresar el nuevo peso de simulación")]
        [Range(0.0001, 1.0000, ErrorMessage = "El peso debe estar entre 0.0001 y 1.0000")]
        public decimal? PesoSimulacion { get; set; }

        public string Name { get; set; } = string.Empty; // Nombre del macroindicador

    }
}
