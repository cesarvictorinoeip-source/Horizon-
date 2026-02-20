using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class ReturnRateConfigConfiguration : IEntityTypeConfiguration<ReturnRateConfig>
    {
        public void Configure(EntityTypeBuilder<ReturnRateConfig> builder)
        {
            builder.ToTable("ReturnRateConfigs");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name).IsRequired().HasMaxLength(100);

            // Configuración de precisión para porcentajes/tasas
            builder.Property(r => r.MinRate).HasPrecision(5, 2);
            builder.Property(r => r.MaxRate).HasPrecision(5, 2);
        }
    }
}