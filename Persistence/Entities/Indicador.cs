using Persistence.Common;
using System;
using System.Collections.Generic;

namespace Persistence.Entities
{
    public class Indicador : BasicEntities<int>
    {
        public required int CountryId { get; set; }
        public required int MacroIndicatorId { get; set; }
        public required decimal Value { get; set; }
        public required int Year { get; set; }

        // CORRECCIÓN: Un indicador pertenece a UN solo país y UN solo macro indicador.
        // No deben ser ICollection, sino el objeto virtual directo.
        public virtual Country? Country { get; set; }
        public virtual MacroIndicador? MacroIndicator { get; set; }
    }
}