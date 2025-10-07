using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab2.Data;
using Lab2.Models;

namespace Lab2.Pages.Carriages
{
    public class DeleteModel : PageModel
    {
        private readonly RailwayContext _context;

        public DeleteModel(RailwayContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Carriage Carriage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Carriage = await _context.Carriages
                .Include(c => c.Train)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Carriage == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // Получаем вагон с поездом
            var carriage = await _context.Carriages
                .Include(c => c.Train)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carriage == null)
                return NotFound();

            var trainId = carriage.TrainId;

            // Удаляем вагон
            _context.Carriages.Remove(carriage);
            await _context.SaveChangesAsync();

            // Перенумерация оставшихся вагонов этого поезда
            var remainingCarriages = await _context.Carriages
                .Where(c => c.TrainId == trainId)
                .OrderBy(c => c.CarriageNumber)
                .ToListAsync();

            for (int i = 0; i < remainingCarriages.Count; i++)
            {
                remainingCarriages[i].CarriageNumber = i + 1;
            }

            var trains = await _context.Trains
                .Include(t => t.Carriages)
                .ToListAsync();

            foreach (var t in trains)
            {
                t.TicketsAvailable = t.Carriages.Sum(c => c.AvailableSeats);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
