using Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities
{
    // 1. Recomendado: Heredar de BasicEntities para estandarizar el Id
    public class Country : BasicEntities<int>
    {
        public required string Name { get; set; }
        public required string IsoCode { get; set; }

        // 2. Relación para los datos anuales (la que ya tenías)
        public virtual ICollection<Indicador>? Indicadores { get; set; }

        // 3. SOLUCIÓN AL ERROR: Esta propiedad DEBE existir para que la configuración funcione
        // Debe llamarse igual que en CountryEntityConfiguration.cs
        public virtual ICollection<CountryMacroIndicator> CountryMacroIndicators { get; set; } = new List<CountryMacroIndicator>();
    }
}