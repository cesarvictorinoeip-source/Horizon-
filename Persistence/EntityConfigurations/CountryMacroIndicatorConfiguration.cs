using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    // Esta es la clase que configura la relación Muchos a Muchos
    public class CountryMacroIndicatorConfiguration : IEntityTypeConfiguration<CountryMacroIndicator>
    {
        public void Configure(EntityTypeBuilder<CountryMacroIndicator> builder)
        {
            builder.ToTable("CountryMacroIndicators");

            // LLAVE PRIMARIA COMPUESTA (Igual que en el ejemplo de User)
            builder.HasKey(x => new { x.CountryId, x.MacroIndicatorId });

            // Configuración de relaciones
            builder.HasOne(x => x.Country)
                   .WithMany(c => c.CountryMacroIndicators)
                   .HasForeignKey(x => x.CountryId);

            builder.HasOne(x => x.MacroIndicator)
                   .WithMany(m => m.CountryMacroIndicators)
                   .HasForeignKey(x => x.MacroIndicatorId);
        }
    }
}