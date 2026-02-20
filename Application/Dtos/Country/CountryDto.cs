using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Country
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CodigoIso { get; set; } = string.Empty;
    }

}
