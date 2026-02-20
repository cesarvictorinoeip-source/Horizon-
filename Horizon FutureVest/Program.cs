using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddScoped<SimulacionMacroindicadorService>();


var app = builder.Build();

// --- BLOQUE DE CARGA AUTOMÁTICA FORZADA ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // 1. Aseguramos que la base de datos exista
    context.Database.EnsureCreated();

    // 2. Si no hay Macroindicadores, los creamos por SQL para que no fallen los tipos
    if (!context.MacroIndicadores.Any())
    {
        context.Database.ExecuteSqlRaw("INSERT INTO MacroIndicadores (Name, Weight, IsHigherBetter) VALUES ('PIB', 0.50, 1)");
        context.Database.ExecuteSqlRaw("INSERT INTO MacroIndicadores (Name, Weight, IsHigherBetter) VALUES ('inflacion', 0.50, 0)");
    }

    // 3. CREACIÓN DE PAÍSES Y DATOS PARA 2024 (Para que el botón funcione)
    if (!context.Countries.Any())
    {
        // Creamos México y USA
        context.Database.ExecuteSqlRaw("INSERT INTO Countries (Name, IsoCode) VALUES ('México', 'MX')");
        context.Database.ExecuteSqlRaw("INSERT INTO Countries (Name, IsoCode) VALUES ('Estados Unidos', 'US')");
        context.SaveChanges();

        // Insertamos datos para 2024 para que el simulador los vea
        context.Database.ExecuteSqlRaw(@"
            INSERT INTO Indicadores (CountryId, MacroIndicatorId, Value, Year)
            SELECT c.Id, m.Id, 2.5, 2024 FROM Countries c, MacroIndicadores m 
            WHERE c.IsoCode = 'MX' AND m.Name = 'PIB'");

        context.Database.ExecuteSqlRaw(@"
            INSERT INTO Indicadores (CountryId, MacroIndicatorId, Value, Year)
            SELECT c.Id, m.Id, 4.0, 2024 FROM Countries c, MacroIndicadores m 
            WHERE c.IsoCode = 'MX' AND m.Name = 'inflacion'");
    }
}
// --- FIN DEL BLOQUE ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();