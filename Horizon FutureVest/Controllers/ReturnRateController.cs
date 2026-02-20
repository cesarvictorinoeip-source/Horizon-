using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.Dtos.ReturnRate;
using Application.ViewModels.ReturnRate;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Horizon_FutureVest.Controllers
{
    // Asegúrate de que esta clase herede de Controller
    public class ReturnRateController : Controller
    {
        private readonly ReturnRateService _returnRateService;

        public ReturnRateController(AppDbContext context)
        {
            var repository = new ReturnRateRepository(context);
            _returnRateService = new ReturnRateService(repository);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dtos = await _returnRateService.GetAllAsync();
            var configuracion = dtos.FirstOrDefault();

            var viewModel = new ReturnRateViewModels();

            if (configuracion != null)
            {
                viewModel.Id = configuracion.Id;
                viewModel.MinRate = configuracion.MinRate;
                viewModel.MaxRate = configuracion.MaxRate;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ReturnRateViewModels model)
        {
            // 1. VALIDACIÓN DEL MANDATO: El error que vimos en tu captura
            if (model.MinRate >= model.MaxRate)
            {
                ModelState.AddModelError("MinRate", "La tasa mínima debe ser menor a la tasa máxima.");
            }

            if (ModelState.IsValid)
            {
                var dto = new ReturnRateDto
                {
                    Id = model.Id,
                    MinRate = model.MinRate,
                    MaxRate = model.MaxRate,
                    Name = "Configuración General"
                };

                if (model.Id == 0)
                {
                    await _returnRateService.AddAsync(dto);
                }
                else
                {
                    await _returnRateService.UpdateAsync(model.Id, dto);
                }

                TempData["Success"] = "Tasas guardadas con éxito.";
                return RedirectToAction(nameof(Index));
            }

            // Si hay error de validación, volvemos a la vista con los mensajes
            return View(model);
        }
    }
}