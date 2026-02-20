using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulacion2._0
{
    public class SimulacionMacroindicadorViewModel
    {
        public int Id { get; set; }
        public int MacroindicadorId { get; set; }
        public string Name { get; set; } = string.Empty; // Nombre del macroindicador
        public decimal PesoSimulacion { get; set; }

    }
}
