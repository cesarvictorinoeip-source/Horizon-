using Persistence.Common;
using System;
using System.Collections.Generic;

namespace Persistence.Entities
{
    public class MacroIndicador : BasicEntities<int>
    {
        public  string Name { get; set; }
        public required decimal Weight { get; set; }
        public required bool IsHigherBetter { get; set; }

        public ICollection<Indicador>? Indicadores { get; set; }
        public ICollection<SimulacionMacroindicador>? Simulaciones { get; set; }

        public virtual ICollection<CountryMacroIndicator> CountryMacroIndicators { get; set; } = new List<CountryMacroIndicator>();
    }
}