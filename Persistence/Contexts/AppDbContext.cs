using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<MacroIndicador> MacroIndicadores { get; set; }

        // Esta es la tabla clave que contiene 'Year' y 'Value'
        public DbSet<Indicador> Indicadores { get; set; }

        public DbSet<ReturnRateConfig> ReturnRateConfigs { get; set; }

        // Tabla de vinculación (Muchos a Muchos)
        public DbSet<CountryMacroIndicator> CountryMacroIndicators { get; set; }

        public DbSet<SimulacionMacroindicador> SimulacionMacroindicadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo explícito basado en tu esquema de DB
            modelBuilder.Entity<Indicador>().ToTable("Indicadores");
            modelBuilder.Entity<CountryMacroIndicator>().ToTable("CountryMacroIndicators");

            // Configuración de clave compuesta para la tabla de nexo
            modelBuilder.Entity<CountryMacroIndicator>()
                .HasKey(c => new { c.CountryId, c.MacroIndicatorId });

            // CARGA TODO AUTOMÁTICAMENTE DESDE TU CARPETA EntityConfigurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}