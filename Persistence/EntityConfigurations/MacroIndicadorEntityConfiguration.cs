using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class MacroIndicadorEntityConfiguration : IEntityTypeConfiguration<MacroIndicador>
    {
        public void Configure(EntityTypeBuilder<MacroIndicador> builder)
        {
            // 1. Nombre de la tabla correcta
            builder.ToTable("MacroIndicadores");

            // 2. Llave primaria simple
            builder.HasKey(m => m.Id);

            // 3. Configuración de propiedades
            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            // 4. PRECISIÓN DEL PESO (Crucial para tus cálculos de ranking)
            builder.Property(m => m.Weight)
                   .HasPrecision(5, 2);

            // 5. Relación con la tabla intermedia (CountryMacroIndicators)
            builder.HasMany(m => m.CountryMacroIndicators)
                   .WithOne(x => x.MacroIndicator)
                   .HasForeignKey(x => x.MacroIndicatorId);
        }
    }
}