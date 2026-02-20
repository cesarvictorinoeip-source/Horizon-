using System;
using Application.ViewModels;

using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.ReturnRate
{
    public class ReturnRateViewModels
    {
        // El ID es necesario para saber si estamos editando o creando
        public int Id { get; set; }

        [Required(ErrorMessage = "La tasa mínima es requerida")]
        [Display(Name = "Tasa Mínima Estimada de Retorno")]
        public decimal MinRate { get; set; }

        [Required(ErrorMessage = "La tasa máxima es requerida")]
        [Display(Name = "Tasa Máxima Estimada de Retorno")]
        public decimal MaxRate { get; set; }

        // Propiedad opcional por si tu DTO pide un nombre
        public string? Name { get; set; }
    }
}