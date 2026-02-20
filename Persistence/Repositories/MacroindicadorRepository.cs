using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;
using Persistence.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class MacroindicadorRepository : IRepository<MacroIndicador>
    {
        private readonly AppDbContext _context;
        public MacroindicadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MacroIndicador entity)
        {
            await _context.Set<MacroIndicador>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<MacroIndicador>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<MacroIndicador>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MacroIndicador>> GetAllAsync()
        {
            return await _context.Set<MacroIndicador>().ToListAsync();
        }

        public async Task<MacroIndicador?> GetByIdAsync(int id)
        {
            return await _context.Set<MacroIndicador>().FindAsync(id);
        }

        public async Task UpdateAsync(MacroIndicador entity)
        {
            var entry = await _context.Set<MacroIndicador>().FindAsync(entity.Id);
            if (entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
            

    }
}
