using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;

public class CountriesController : Controller
{
    private readonly AppDbContext _context;

    public CountriesController(AppDbContext context)
    {
        _context = context;
    }

    // LISTADO DE PAÍSES
    public async Task<IActionResult> Index()
    {
        var paises = await _context.Countries
            .Select(c => new CountryViewModels
            {
                Id = c.Id,
                Name = c.Name,
                IsoCode = c.IsoCode
            })
            .ToListAsync();

        return View(paises);
    }

    // CREAR PAÍS (GET)
    public IActionResult Create()
    {
        return View();
    }

    // CREAR PAÍS (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CountryViewModels vm)
    {
        if (ModelState.IsValid)
        {
            var country = new Country
            {
                Id= vm.Id,
                Name = vm.Name,
                IsoCode = vm.IsoCode
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    // EDITAR PAÍS (GET)
    public async Task<IActionResult> Edit(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null) return NotFound();

        var vm = new CountryViewModels
        {
            Id = country.Id,
            Name = country.Name,
            IsoCode = country.IsoCode
        };

        return View(vm);
    }

    // EDITAR PAÍS (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CountryViewModels vm)
    {
        if (id != vm.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();

            country.Name = vm.Name;
            country.IsoCode = vm.IsoCode;

            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    // ELIMINAR PAÍS (GET - Confirmación)
    public async Task<IActionResult> Delete(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null) return NotFound();

        var vm = new CountryViewModels
        {
            Id = country.Id,
            Name = country.Name,
            IsoCode = country.IsoCode
        };

        return View(vm);
    }

    // ELIMINAR PAÍS (POST - Confirmado)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null) return NotFound();

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}