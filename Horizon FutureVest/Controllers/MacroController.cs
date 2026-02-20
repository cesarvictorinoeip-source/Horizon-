using Microsoft.AspNetCore.Mvc;
using Application.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Horizon_FutureVest.Controllers
{
    public class MacroController : Controller
    {
        private readonly AppDbContext _context;

        public MacroController(AppDbContext context)
        {
            _context = context;
        }

        // LISTADO DE MACROINDICADORES
        public async Task<IActionResult> Index()
        {
            var macros = await _context.MacroIndicadores
                .Select(m => new MacroindicadorViewModel
                {
                    Id = m.Id,
                    Nombre = m.Name,
                    Peso = m.Weight,
                    MasAltoMejor = m.IsHigherBetter
                })
                .ToListAsync();

            return View(macros);
        }

        public async Task<IActionResult> Create()
        {
            var sumaPesos = await _context.MacroIndicadores.SumAsync(m => m.Weight);
            if (sumaPesos >= 1)
            {
                TempData["Error"] = "No se pueden crear más macroindicadores, la suma de pesos ya es 1.";
                return RedirectToAction(nameof(Index));
            }

            // ✅ Aquí inicializamos el modelo para que la vista nunca reciba null
            return View(new MacroindicadorViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MacroindicadorViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var sumaPesos = await _context.MacroIndicadores.SumAsync(m => m.Weight);
                if (sumaPesos + vm.Peso > 1)
                {
                    ModelState.AddModelError("Peso", "La suma de los pesos no puede superar 1.");
                    return View(vm);
                }

                var macro = new MacroIndicador
                {
                    Name = vm.Nombre,
                    Weight = vm.Peso,
                    IsHigherBetter = vm.MasAltoMejor
                };

                _context.MacroIndicadores.Add(macro);
                await _context.SaveChangesAsync();

                // ✅ Redirigir al listado después de crear
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores de validación, se regresa la vista con el modelo
            return View(vm);
        }

        // EDITAR MACROINDICADOR (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var macro = await _context.MacroIndicadores.FindAsync(id);
            if (macro == null) return NotFound();

            var vm = new MacroindicadorViewModel
            {
                Id = macro.Id,
                Nombre = macro.Name,
                Peso = macro.Weight,
                MasAltoMejor = macro.IsHigherBetter
            };

            return View(vm);
        }

        // EDITAR MACROINDICADOR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MacroindicadorViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var sumaPesos = await _context.MacroIndicadores
                    .Where(m => m.Id != id)
                    .SumAsync(m => m.Weight);

                if (sumaPesos + vm.Peso > 1)
                {
                    ModelState.AddModelError("Peso", "La suma de los pesos no puede superar 1.");
                    return View(vm);
                }

                var macro = await _context.MacroIndicadores.FindAsync(id);
                if (macro == null) return NotFound();

                macro.Name = vm.Nombre;
                macro.Weight = vm.Peso;
                macro.IsHigherBetter = vm.MasAltoMejor;

                _context.MacroIndicadores.Update(macro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // ELIMINAR MACROINDICADOR (GET - Confirmación)
        public async Task<IActionResult> Delete(int id)
        {
            var macro = await _context.MacroIndicadores.FindAsync(id);
            if (macro == null) return NotFound();

            var vm = new MacroindicadorViewModel
            {
                Id = macro.Id,
                Nombre = macro.Name,
                Peso = macro.Weight,
                MasAltoMejor = macro.IsHigherBetter
            };

            return View(vm);
        }

        // ELIMINAR MACROINDICADOR (POST - Confirmado)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var macro = await _context.MacroIndicadores.FindAsync(id);
            if (macro == null) return NotFound();

            _context.MacroIndicadores.Remove(macro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}