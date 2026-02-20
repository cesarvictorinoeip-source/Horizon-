using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Simulacion
{
    public class SimulacionFormViewModel
    {
        // Este se queda como int porque es el ID interno
        public int MacroindicadorId { get; set; }

        // AQUÍ VA EL STRING: Es necesario para que el formulario 
        // pueda mostrar nombres como "PIB" o "Inflación"
        public string NombreMacro { get; set; } = string.Empty;

        // Este se queda como decimal para el cálculo matemático
        public decimal Peso { get; set; }
    }
}