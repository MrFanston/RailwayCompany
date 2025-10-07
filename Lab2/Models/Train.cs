using System;
using System.ComponentModel.DataAnnotations;

namespace Lab2.Models
{
    public class Train
    {
        public List<Carriage> Carriages { get; set; } = new List<Carriage>();

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Номер поезда")]
        public string TrainNumber => $"П-{Id:D3}";

        [Required]
        [Display(Name = "Станция назначения")]
        public string Destination { get; set; }

        [Display(Name = "Время отправления")]
        public DateTime DepartureTime { get; set; }

        [Display(Name = "Время в пути (часы)")]
        public TimeSpan TravelTime => ArrivalTime - DepartureTime;

        [Display(Name = "Время прибытия")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Наличие билетов")]
        public int TicketsAvailable { get; set; }

        [Display(Name = "Количество вагонов")]
        public int CarriageCount { get; set; }
    }
}
