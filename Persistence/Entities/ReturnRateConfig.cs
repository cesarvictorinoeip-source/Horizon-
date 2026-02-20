using Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities
{
    public class ReturnRateConfig  : BasicEntities<int>
    {
        public  string Name { get; set; } // Ejemplo: "Tasa Base", "Tasa de Riesgo"
        public  decimal MinRate { get; set; }
        public  decimal MaxRate { get; set; }
    }
}