using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities
{
    public class SimulacionMacroindicador
    {
        public int Id { get; set; }

        public required int MacroindicadorId { get; set; }

        public required decimal PesoSimulacion { get; set; }

        // Navigation

        public MacroIndicador? Macroindicador { get; set; }

    }
}
