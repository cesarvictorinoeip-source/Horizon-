using Application.Dtos.Country;
using Application.Dtos.Indicador;
using Application.Dtos.Simulacion;
using Application.Services;
using Application.ViewModels.Simulacion2._0;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Contexts;
using Persistence.Entities;

namespace Horizon_FutureVest.Controllers
{
    public class SimulacionController : Controller
    {

        private readonly AppDbContext _context;
        private readonly SimulacionMacroindicadorService _simulacionService;

       public SimulacionController(AppDbContext context, SimulacionMacroindicadorService simulacionService)
        {
            _context = context;
            _simulacionService = simulacionService;
        }


        public async Task<IActionResult> Index()
        {
            var simulados = await _simulacionService.GetAllAsync();
            var años = await _context.Indicadores
            .Select(i => i.Year)          // Tomamos solo el campo Year
            .Distinct()                   // Quitamos duplicados
            .OrderByDescending(y => y)    // Ordenamos descendente (más reciente primero)
            .ToListAsync();               // Lo convertimos en lista

            var vm = new SimulacionRankingViewModel
            {
                MacroindicadoresSimulados = simulados.Select(s => new SimulacionMacroindicadorViewModel
                {
                    Id = s.Id,
                    MacroindicadorId = s.MacroindicadorId,
                    Name = s.NombreMacroindicador,
                    PesoSimulacion = s.PesoSimulacion
                }).ToList(),
                AñosDisponibles = años.Select(a => new SelectListItem
                {
                    Value = a.ToString(),
                    Text = a.ToString()
                }).ToList(),
                AnioSeleccionado = años.FirstOrDefault()
            };


            return View(vm);
        }

        public async Task<IActionResult> Agregar()
        {
            var simulados = await _simulacionService.GetAllAsync();
            var usados = simulados.Select(s => s.MacroindicadorId).ToList();
            var disponibles = await _simulacionService.GetMacroindicadoresDisponiblesAsync(usados);

            var vm = new AgregarSimulacionMacroindicadorViewModel
            {
                MacroindicadoresDisponibles = disponibles.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(AgregarSimulacionMacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.MacroindicadoresDisponibles = await GetSelectListAsync();
                return View(vm);
            }

            var dto = new SimulacionMacroindicadorInputDto
            {
                MacroindicadorId = vm.MacroindicadorId,
                PesoSimulacion = vm.PesoSimulacion
            };

            var ok = await _simulacionService.CrearAsync(dto);
            if (!ok)
            {
                ModelState.AddModelError("", "No se pudo agregar el macroindicador. Verifica que no esté duplicado y que el peso total no supere 1.");
                vm.MacroindicadoresDisponibles = await GetSelectListAsync();
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var simulados = await _simulacionService.GetAllAsync();
            var actual = simulados.FirstOrDefault(s => s.Id == id);
            if (actual == null) return NotFound();

            var vm = new EditarSimulacionMacroindicadorViewModel
            {
                Id = actual.Id,
                PesoSimulacion = actual.PesoSimulacion,
                Name = actual.NombreMacroindicador
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EditarSimulacionMacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new SimulacionMacroindicadorInputDto
            {
                MacroindicadorId = vm.Id,
                PesoSimulacion = vm.PesoSimulacion
            };

            var ok = await _simulacionService.UpdateAsync(vm.Id, dto);
            if (!ok)
            {
                ModelState.AddModelError("", "No se pudo actualizar el peso. Verifica que la suma total no supere 1.");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var simulados = await _simulacionService.GetAllAsync();
            var actual = simulados.FirstOrDefault(s => s.Id == id);
            if (actual == null) return NotFound();

            var vm = new SimulacionMacroindicadorViewModel
            {
                Id = actual.Id,
                MacroindicadorId = actual.MacroindicadorId,
                Name = actual.NombreMacroindicador,
                PesoSimulacion = actual.PesoSimulacion
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(SimulacionMacroindicadorViewModel vm)
        {
            await _simulacionService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> SimularRanking(SimulacionRankingViewModel vm)
        {
            // Traer países desde la BD y proyectarlos a entidades
            var paises = await _context.Countries
                .Select(p => new Persistence.Entities.Country
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsoCode = p.IsoCode
                })
                .ToListAsync();

            // Traer indicadores desde la BD y proyectarlos a entidades
            var indicadores = await _context.Indicadores
                .Select(i => new Persistence.Entities.Indicador
                {
                    Id = i.Id,
                    CountryId = i.CountryId,
                    MacroIndicatorId = i.MacroIndicatorId,
                    Value = i.Value,
                    Year = i.Year
                })
                .ToListAsync();

            // Ahora sí puedes llamar al servicio con entidades
            var resultado = await _simulacionService.GenerarSimulacionAsync(
                vm.AnioSeleccionado!.Value,
                indicadores,
                paises
            );

            vm.ResultadoRanking = resultado.Ranking;
            vm.Mensaje = resultado.Mensaje;

            // Macroindicadores simulados
            var simulados = await _simulacionService.GetAllAsync();
            vm.MacroindicadoresSimulados = simulados.Select(s => new SimulacionMacroindicadorViewModel
            {
                Id = s.Id,
                MacroindicadorId = s.MacroindicadorId,
                Name = s.NombreMacroindicador,
                PesoSimulacion = s.PesoSimulacion
            }).ToList();

            // Lista de años disponibles
            vm.AñosDisponibles = (await _context.Indicadores
                .Select(i => i.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync())
                .Select(a => new SelectListItem { Value = a.ToString(), Text = a.ToString() })
                .ToList();

            return View("Index", vm);
        }
        private async Task<List<SelectListItem>> GetSelectListAsync()
        {
            var simulados = await _simulacionService.GetAllAsync();
            var usados = simulados.Select(s => s.MacroindicadorId).ToList();
            var disponibles = await _simulacionService.GetMacroindicadoresDisponiblesAsync(usados);

            return disponibles.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToList();
        }

    }
}
