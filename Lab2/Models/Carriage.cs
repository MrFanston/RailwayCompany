using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lab2.Models
{
    public class Carriage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Выберите поезд")]
        [Display(Name = "Номер поезда")]
        public int TrainId { get; set; }

        [ForeignKey("TrainId")]
        [ValidateNever]
        public Train Train { get; set; }

        [Display(Name = "Номер вагона")]
        public int CarriageNumber { get; set; }

        [Required(ErrorMessage = "Укажите тип вагона")]
        [Display(Name = "Тип вагона")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Range(1, 200)]
        [Display(Name = "Количество мест")]
        public int SeatCount { get; set; }

        [Required]
        [Range(0, 200)]
        [Display(Name = "Осталось билетов")]
        public int AvailableSeats { get; set; }

        [Display(Name = "Наличие кондиционера")]
        public bool HasAirConditioning { get; set; }

        [Display(Name = "Наличие Wi-Fi")]
        public bool HasWiFi { get; set; }
    }
}
