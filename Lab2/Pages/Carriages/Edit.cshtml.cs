using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Pages.Carriages
{
    public class EditModel : PageModel
    {
        private readonly Lab2.Data.RailwayContext _context;

        public List<Train> Trains { get; set; } = new List<Train>();

        public EditModel(Lab2.Data.RailwayContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Carriage Carriage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carriage =  await _context.Carriages.FirstOrDefaultAsync(m => m.Id == id);
            if (carriage == null)
            {
                return NotFound();
            }
            Carriage = carriage;
           ViewData["TrainId"] = new SelectList(_context.Trains, "Id", "TrainNumber");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Carriage.AvailableSeats > Carriage.SeatCount)
            {
                ModelState.AddModelError(nameof(Carriage.AvailableSeats),
                    "Количество свободных мест не может превышать общее количество мест в вагоне.");
                return Page();
            }

            _context.Attach(Carriage).State = EntityState.Modified;

            // Подгружаем список поездов
            Trains = await _context.Trains.Include(t => t.Carriages).ToListAsync();

            // Получаем поезд выбранный пользователем
            var train = await _context.Trains
                .Include(t => t.Carriages)
                .FirstOrDefaultAsync(t => t.Id == Carriage.TrainId);

            // Подсчёт доступных билетов для каждого поезда
            train.TicketsAvailable = train.Carriages.Sum(c => c.AvailableSeats);

            _context.Attach(train).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarriageExists(Carriage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarriageExists(int id)
        {
            return _context.Carriages.Any(e => e.Id == id);
        }
    }
}
