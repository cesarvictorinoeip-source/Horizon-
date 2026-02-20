using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Simulacion
{
    public class SimulacionMacroindicadorDto
    {
        public int Id { get; set; }                      // Id del registro en la simulación
        public int MacroindicadorId { get; set; }        // Id del macroindicador original
        public decimal PesoSimulacion { get; set; }      // Peso asignado en la simulación (decimal 5,4)

        // Opcional para mostrar en la vista
        public string? NombreMacroindicador { get; set; } // Nombre del macroindicador (para mostrar)

    }
}
