using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Countries");

            // Llave primaria
            builder.HasKey(c => c.Id);

            // Configuración de propiedades
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.IsoCode)
                .IsRequired()
                .HasMaxLength(5);

            // Índice único para evitar países duplicados por código ISO
            builder.HasIndex(c => c.IsoCode).IsUnique();

            // RELACIÓN: Un país tiene muchos CountryMacroIndicators
            builder.HasMany(c => c.CountryMacroIndicators)
                   .WithOne(x => x.Country)
                   .HasForeignKey(x => x.CountryId)
                   .OnDelete(DeleteBehavior.Cascade); // <--- AGREGADO: Borrado en cascada
        }
    }
}