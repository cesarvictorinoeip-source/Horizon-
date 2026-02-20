using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulacion
{
    public class SimulacionConfigViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Peso { get; set; }
    }
}