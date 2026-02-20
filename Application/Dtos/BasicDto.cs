using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    // Agregamos <T> para que sea una clase genérica
    public class BasicDto<Tkey>
    {
        // El Id será del tipo T (en tu caso será int)
        public required Tkey Id { get; set; }

        // Propiedad común para todas tus entidades (País, Tasas, etc.)
        public string Name { get; set; } = string.Empty;
    }
}