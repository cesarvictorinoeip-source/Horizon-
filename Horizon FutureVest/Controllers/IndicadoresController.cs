using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;

namespace Horizon_FutureVest.Controllers
{
    public class IndicadoresController : Controller
    {
        private readonly AppDbContext _context;

        public IndicadoresController(AppDbContext context)
        {
            _context = context;
        }


        // GET: Indicadores
        public async Task<IActionResult> Index(int? paisId, int? anio)
        {
            var query = _context.Indicadores
                .Include(i => i.Country)
                .Include(i => i.MacroIndicator)
                .AsQueryable();

            if (paisId.HasValue)
                query = query.Where(i => i.CountryId == paisId.Value);

            if (anio.HasValue)
                query = query.Where(i => i.Year == anio.Value);

            var indicadores = await query
                .Select(i => new IndicadorViewModels
                {
                    Id = i.Id,
                    PaisId = i.CountryId,
                    PaisNombre = i.Country!.Name,
                    MacroindicadorId = i.MacroIndicatorId,
                    MacroindicadorNombre = i.MacroIndicator!.Name,
                    Valor = i.Value,
                    Anio = i.Year
                }).ToListAsync();

            ViewBag.Paises = new SelectList(await _context.Countries.ToListAsync(), "Id", "Name");
            return View(indicadores);
        }

        // GET: Indicadores/Create
        public async Task<IActionResult> Create()
        {
            var vm = new IndicadorViewModels
            {
                Paises = (await _context.Countries.ToListAsync())
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList(),
                Macroindicadores = (await _context.MacroIndicadores.ToListAsync())
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList()
            };
            return View(vm);
        }

        // POST: Indicadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndicadorViewModels vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Paises = (await _context.Countries.ToListAsync())
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                vm.Macroindicadores = (await _context.MacroIndicadores.ToListAsync())
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
                return View(vm);
            }

            // Validación: no duplicar macroindicador/año para el mismo país
            bool existe = await _context.Indicadores.AnyAsync(i =>
                i.CountryId == vm.PaisId &&
                i.MacroIndicatorId == vm.MacroindicadorId &&
                i.Year == vm.Anio);

            if (existe)
            {
                ModelState.AddModelError("", "Ya existe un indicador para ese país, macroindicador y año.");
                vm.Paises = (await _context.Countries.ToListAsync())
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                vm.Macroindicadores = (await _context.MacroIndicadores.ToListAsync())
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
                return View(vm);
            }

            var indicador = new Indicador
            {
                CountryId = vm.PaisId,
                MacroIndicatorId = vm.MacroindicadorId,
                Value = vm.Valor,
                Year = vm.Anio
            };

            _context.Add(indicador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Indicadores/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var indicador = await _context.Indicadores
                .Include(i => i.Country)
                .Include(i => i.MacroIndicator)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (indicador == null) return NotFound();

            var vm = new IndicadorViewModels
            {
                Id = indicador.Id,
                PaisId = indicador.CountryId,
                PaisNombre = indicador.Country?.Name,
                MacroindicadorId = indicador.MacroIndicatorId,
                MacroindicadorNombre = indicador.MacroIndicator?.Name,
                Valor = indicador.Value,
                Anio = indicador.Year
            };

            return View(vm);
        }

        // POST: Indicadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IndicadorViewModels vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid) return View(vm);

            var indicador = await _context.Indicadores.FindAsync(id);
            if (indicador == null) return NotFound();

            // Solo se edita el valor
            indicador.Value = vm.Valor;

            _context.Update(indicador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Indicadores/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var indicador = await _context.Indicadores
                .Include(i => i.Country)
                .Include(i => i.MacroIndicator)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (indicador == null) return NotFound();

            var vm = new IndicadorViewModels
            {
                Id = indicador.Id,
                PaisNombre = indicador.Country?.Name,
                MacroindicadorNombre = indicador.MacroIndicator?.Name,
                Valor = indicador.Value,
                Anio = indicador.Year
            };

            return View(vm);
        }

        // POST: Indicadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var indicador = await _context.Indicadores.FindAsync(id);
            if (indicador == null) return NotFound();

            _context.Indicadores.Remove(indicador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}