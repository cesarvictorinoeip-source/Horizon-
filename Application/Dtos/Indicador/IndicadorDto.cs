using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Indicador
{
    public class IndicadorDto
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int MacroIndicatorId { get; set; }

        // Usa los nombres que realmente existen en tu BD
        public decimal Valor { get; set; }   // corresponde a la columna "Valor"
        public int Anio { get; set; }        // corresponde a la columna "Anio"
    }

}
