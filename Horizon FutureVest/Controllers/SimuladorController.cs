using Microsoft.AspNetCore.Mvc;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Application.ViewModels.Simulacion;

namespace Horizon_FutureVest.Controllers
{
    public class SimuladorController : Controller
    {
        private readonly AppDbContext _context;
        private static List<SimulacionConfigViewModel> _configuracionActual = new List<SimulacionConfigViewModel>();

        public SimuladorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            decimal sumaTotal = _configuracionActual.Sum(s => s.Peso);
            ViewBag.SumaTotal = sumaTotal;
            ViewBag.PuedeAgregar = sumaTotal < 1.0m;

            if (TempData["Error"] != null) ViewBag.ErrorMessage = TempData["Error"];

            return View(_configuracionActual);
        }

        [HttpGet]
        public IActionResult Agregar() => View(new SimulacionFormViewModel());

        [HttpPost]
        public IActionResult Agregar(SimulacionFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                _configuracionActual.Add(new SimulacionConfigViewModel
                {
                    Id = _configuracionActual.Count + 1,
                    Nombre = model.NombreMacro.Trim(), // Limpiamos espacios
                    Peso = model.Peso
                });
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Cambiado a 2023 por defecto para coincidir con tus datos de SQL
        public IActionResult GenerarRanking(int anio = 2023)
        {
            if (Math.Abs(_configuracionActual.Sum(s => s.Peso) - 1.0m) > 0.001m)
            {
                TempData["Error"] = "La suma de los pesos debe ser exactamente 1.0 (100%).";
                return RedirectToAction("Index");
            }

            var datosDB = _context.Indicadores
                .Include(i => i.Country)
                .Include(i => i.MacroIndicator)
                .Where(i => i.Year == anio)
                .ToList();

            var resultados = datosDB
                .GroupBy(i => i.Country?.Name ?? "País Desconocido")
                .Select(g => new //RankingViewModel
                {
                    PaisNombre = g.Key,
                    // Calculamos el scoring basado en los indicadores configurados
                    Scoring = g.Sum(dato =>
                    {
                        var config = _configuracionActual.FirstOrDefault(c =>
                            string.Equals(c.Nombre.Trim(), dato.MacroIndicator?.Name?.Trim(), StringComparison.OrdinalIgnoreCase));

                        if (config != null)
                        {
                            bool esMejorAlto = dato.MacroIndicator?.IsHigherBetter ?? true;
                            // Normalización base 10 como tienes en tu lógica original
                            decimal puntajeBase = esMejorAlto ? (dato.Value / 10m) : (1m - (dato.Value / 10m));
                            return Math.Max(0, puntajeBase * config.Peso);
                        }
                        return 0;
                    })
                })
                .Where(r => r.Scoring > 0)
                .OrderByDescending(r => r.Scoring)
                .ToList();

            if (!resultados.Any())
            {
                TempData["Error"] = $"No se encontraron datos para el año {anio}. Asegúrate de escribir 'Inflación' con tilde si así está en la base de datos.";
                return RedirectToAction("Index");
            }

            return View("RankingResultados", resultados);
        }

        [HttpPost]
        public IActionResult ConfirmarEliminar(int id)
        {
            _configuracionActual.RemoveAll(c => c.Id == id);
            return RedirectToAction("Index");
        }
    }
}