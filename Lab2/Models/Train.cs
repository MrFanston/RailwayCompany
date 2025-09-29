using System;
using System.ComponentModel.DataAnnotations;

namespace Lab2.Models
{
    public class Train
    {
        public int Id { get; set; } // Primary Key

        [Required]
        [Display(Name = "Номер поезда")]
        public string TrainNumber { get; set; }

        [Required]
        [Display(Name = "Станция назначения")]
        public string Destination { get; set; }

        [Display(Name = "Время отправления")]
        public DateTime DepartureTime { get; set; }

        [Display(Name = "Время в пути (часы)")]
        public double TravelTimeHours { get; set; }

        [Display(Name = "Время прибытия")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Наличие билетов")]
        public bool TicketsAvailable { get; set; }

        [Display(Name = "Количество вагонов")]
        public int CarriageCount { get; set; }
    }
}
