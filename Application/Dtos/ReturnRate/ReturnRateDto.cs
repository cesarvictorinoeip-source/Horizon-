using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ReturnRate
{
    public class ReturnRateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal MinRate { get; set; }
        public decimal MaxRate { get; set; }
    }
}