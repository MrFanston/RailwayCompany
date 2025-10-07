using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Pages.Carriages
{
    public class CreateModel : PageModel
    {
        private readonly RailwayContext _context;

        public CreateModel(RailwayContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Carriage Carriage { get; set; }

        public List<Train> Trains { get; set; } = new List<Train>();

        public async Task<IActionResult> OnGetAsync()
        {
            Trains = await _context.Trains.Include(t => t.Carriages).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Подгружаем список поездов
            Trains = await _context.Trains.Include(t => t.Carriages).ToListAsync();

            if (!ModelState.IsValid)
                return Page();

            // Получаем поезд выбранный пользователем
            var train = await _context.Trains
                .Include(t => t.Carriages)
                .FirstOrDefaultAsync(t => t.Id == Carriage.TrainId);

            // Проверка на количество вагонов
            if (train.Carriages.Count >= train.CarriageCount)
            {
                ModelState.AddModelError(nameof(train.Carriages.Count),
                    $"Нельзя создать больше вагонов для поезда {train.TrainNumber}. Максимум: {train.CarriageCount}.");
                return Page();
            }

            if (Carriage.AvailableSeats > Carriage.SeatCount)
            {
                ModelState.AddModelError(nameof(Carriage.AvailableSeats),
                    "Количество свободных мест не может превышать общее количество мест в вагоне.");
                return Page();
            }

            // Автонумерация номера вагона
            Carriage.CarriageNumber = train.Carriages.Any()
                ? train.Carriages.Max(c => c.CarriageNumber) + 1
                : 1;

            _context.Carriages.Add(Carriage);

            // Подсчёт доступных билетов для каждого поезда
            train.TicketsAvailable = train.Carriages.Sum(c => c.AvailableSeats);

            _context.Attach(train).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }


}
