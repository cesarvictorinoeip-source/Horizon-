using Persistence.Contexts;
using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ReturnRateRepository
    {
        private readonly AppDbContext _context;

        public ReturnRateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReturnRateConfig>> GetAllAsync()
        {
            return await _context.Set<ReturnRateConfig>().ToListAsync();
        }

        // Obtener por ID para las vistas de Detalle o Edición
        public async Task<ReturnRateConfig?> GetByIdAsync(int id)
        {
            return await _context.Set<ReturnRateConfig>().FindAsync(id);
        }

        public async Task<ReturnRateConfig> AddAsync(ReturnRateConfig config)
        {
            await _context.Set<ReturnRateConfig>().AddAsync(config);
            await _context.SaveChangesAsync();
            return config; // Retorna el objeto creado como en tu ejemplo
        }

        // UPDATE usando ID y SetValues
        public async Task<ReturnRateConfig?> UpdateAsync(int id, ReturnRateConfig config)
        {
            var entry = await _context.Set<ReturnRateConfig>().FindAsync(id);
            if (entry != null)
            {
                // Mapea los valores nuevos sobre la entidad existente
                _context.Entry(entry).CurrentValues.SetValues(config);
                await _context.SaveChangesAsync();
                return entry;
            }
            return null;
        }

        // DELETE usando ID
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<ReturnRateConfig>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<ReturnRateConfig>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}