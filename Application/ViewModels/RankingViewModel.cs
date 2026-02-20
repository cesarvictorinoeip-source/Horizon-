using System;

namespace Application.ViewModels
{
    public class RankingViewModel
    {
        public int AnioSeleccionado { get; set; }
        public List<int> AniosDisponibles { get; set; } = new List<int>();
        public List<RankingItem> Resultados { get; set; } = new List<RankingItem>();
        public string? MensajeError { get; set; }
        public string? EnlaceMantenimiento { get; set; }
        public string? TextoEnlace { get; set; }
        public decimal Scoring { get; set; }
        public string PaisNombre { get; set; } = string.Empty;
    }

    public class RankingItem
    {
        public string PaisNombre { get; set; } = "";
        public string CodigoIso { get; set; } = "";
        public decimal Scoring { get; set; }
        public decimal TasaRetorno { get; set; }
    }
}