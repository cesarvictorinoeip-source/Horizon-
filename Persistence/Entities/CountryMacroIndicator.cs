namespace Persistence.Entities
{
    public class CountryMacroIndicator
    {
        public required int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public required int MacroIndicatorId { get; set; }
        public virtual MacroIndicador? MacroIndicator { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public string? SourceNotes { get; set; }

        // CAMBIO CRÍTICO: Debe ser 'Year' para que coincida con la DB
        public int Year { get; set; }

        // Asegúrate de que esta columna 'Value' también exista en tu DB
        public decimal Value { get; set; }
    }
}