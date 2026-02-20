using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class HomeRankingViewModel
    {
        // Año seleccionado en el formulario
        [Required(ErrorMessage = "Debe seleccionar un año")]
        public int SelectedYear { get; set; }

        // Lista de años disponibles (para el select en la vista)
        public List<SelectListItem>? AvailableYears { get; set; }

        // Validación de pesos de macroindicadores
        public bool PesosValidos { get; set; }
        public decimal SumaPesos { get; set; }

        // Mensajes de validación (ej: suma de pesos ≠ 1, falta de indicadores, etc.)
        public string? ValidationMessage { get; set; }

        // Resultado del ranking
        public List<RankingResultViewModel> Ranking { get; set; } = new();
    }

    public class RankingResultViewModel
    {
        public string CountryName { get; set; } = string.Empty;
        public string IsoCode { get; set; } = string.Empty;
        public decimal Scoring { get; set; }
        public decimal ReturnRate { get; set; }
    }
}
