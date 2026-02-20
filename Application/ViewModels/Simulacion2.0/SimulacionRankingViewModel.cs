using Application.Dtos.RankinGeneral;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace Application.ViewModels.Simulacion2._0
{
    public class SimulacionRankingViewModel
    {
        public List<SimulacionMacroindicadorViewModel> MacroindicadoresSimulados { get; set; } = new();

        [Required(ErrorMessage = "Debes seleccionar un año")]
        public int? AnioSeleccionado { get; set; }

        public List<SelectListItem> AñosDisponibles { get; set; } = new();

        public string? Mensaje { get; set; }

        public List<RankingGeneralDto> ResultadoRanking { get; set; } = new(); // Si se genera

    }
}
