using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class IndicadorEntityConfiguration : IEntityTypeConfiguration<Indicador>
    {
        public void Configure(EntityTypeBuilder<Indicador> builder)
        {
            builder.ToTable("Indicadores");
            builder.HasKey(i => i.Id);

            // Configuramos la precisión del valor decimal (Ej: 15.1234)
            builder.Property(i => i.Value)
                   .HasPrecision(18, 4);

            // RELACIONES: Un indicador pertenece a un país y a un macroindicador
            builder.HasOne(i => i.Country)
                   .WithMany(c => c.Indicadores)
                   .HasForeignKey(i => i.CountryId);

            builder.HasOne(i => i.MacroIndicator)
                   .WithMany() // No creamos colección en MacroIndicador para esto
                   .HasForeignKey(i => i.MacroIndicatorId);
                   
        }
    }
}