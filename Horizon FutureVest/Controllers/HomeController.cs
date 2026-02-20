using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Horizon_FutureVest.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? year)
        {
            var vm = new HomeRankingViewModel();

            // 1. Obtener años disponibles desde Indicadores
            var years = await _context.Indicadores
                .Select(i => i.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();

            vm.AvailableYears = years
                .Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() })
                .ToList();

            vm.SelectedYear = year ?? years.FirstOrDefault();

            // 2. Validar suma de pesos
            var macros = await _context.MacroIndicadores.Where(m => m.Weight > 0).ToListAsync();
            vm.SumaPesos = macros.Sum(m => m.Weight);
            vm.PesosValidos = vm.SumaPesos == 1m;

            if (!vm.PesosValidos)
            {
                vm.ValidationMessage = "Se deben ajustar los pesos de los macroindicadores hasta que la suma sea igual a 1.";
                return View(vm);
            }

            // 3. Configuración de tasas
            var retornoConfig = await _context.ReturnRateConfigs.FirstOrDefaultAsync();
            var minRate = retornoConfig?.MinRate > 0 ? retornoConfig.MinRate : 2.00m;
            var maxRate = retornoConfig?.MaxRate > 0 ? retornoConfig.MaxRate : 15.00m;

            // 4. Indicadores del año seleccionado
            var indicadoresPorAnio = await _context.Indicadores
                .Include(i => i.Country)
                .Include(i => i.MacroIndicator)
                .Where(i => i.Year == vm.SelectedYear)
                .ToListAsync();

            // 5. Filtrar países elegibles (que tengan todos los macros con peso > 0)
            var paisesElegibles = indicadoresPorAnio
                .GroupBy(i => i.Country!)
                .Where(g => g.Select(i => i.MacroIndicatorId).Distinct().Count() == macros.Count)
                .Select(g => g.Key)
                .ToList();

            if (paisesElegibles.Count < 2)
            {
                vm.ValidationMessage = "No hay suficientes países para calcular el ranking. Debe agregar más indicadores.";
                return View(vm);
            }

            // 6. Calcular scoring y tasa de retorno
            foreach (var pais in paisesElegibles)
            {
                var indicadoresPais = indicadoresPorAnio.Where(i => i.CountryId == pais.Id).ToList();
                decimal scoring = 0;

                foreach (var macro in macros)
                {
                    // min y max: todos los países elegibles
                    var valoresIndicador = paisesElegibles
                        .Select(p => indicadoresPorAnio
                            .Single(i => i.CountryId == p.Id && i.MacroIndicatorId == macro.Id).Value)
                        .ToList();

                    var min = valoresIndicador.Min();
                    var max = valoresIndicador.Max();

                    // valor del país actual
                    var valor = indicadoresPorAnio
                        .Single(i => i.CountryId == pais.Id && i.MacroIndicatorId == macro.Id).Value;

                    decimal normalizado;
                    if (min == max)
                    {
                        normalizado = 0.5m;
                    }
                    else if (macro.IsHigherBetter)
                    {
                        normalizado = (valor - min) / (max - min);
                    }
                    else
                    {
                        normalizado = (max - valor) / (max - min);
                    }

                    scoring += normalizado * macro.Weight;
                }

                scoring = Math.Clamp(scoring, 0, 1);
                var tasaRetorno = minRate + (maxRate - minRate) * scoring;

                vm.Ranking.Add(new RankingResultViewModel
                {
                    CountryName = pais.Name,
                    IsoCode = pais.IsoCode,
                    Scoring = Math.Round(scoring, 4),
                    ReturnRate = Math.Round(tasaRetorno, 2)
                });
            }

            // 7. Ordenar ranking
            vm.Ranking = vm.Ranking.OrderByDescending(r => r.Scoring).ToList();

            return View(vm);
        }


    }
}