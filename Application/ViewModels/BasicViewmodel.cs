using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    // Cambiamos el nombre a BasicViewModel para que coincida con tu herencia
    public class BasicViewModel<Tkey>
    {
        // El Id será del tipo Tkey (int en tu caso)
        public required Tkey Id { get; set; }

        // Como casi todas tus tablas tienen Name, lo dejamos aquí
        public string Name { get; set; } = string.Empty;
    }
}