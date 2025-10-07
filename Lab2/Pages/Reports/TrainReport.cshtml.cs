using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Pages.Reports
{
    public class TrainReportModel : PageModel
    {
        private readonly RailwayContext _context;

        public TrainReportModel(RailwayContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? Station { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool OnlyWithAvailableSeats { get; set; }

        public IList<Train> Trains { get; set; } = new List<Train>();

        // Итоговые показатели
        public int TotalTrains { get; set; }
        public int TotalTickets { get; set; }
        public TimeSpan AverageTravelTime { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Trains
                .Include(t => t.Carriages)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Station))
                query = query.Where(t => t.Destination.Contains(Station));

            if (FromDate.HasValue)
                query = query.Where(t => t.DepartureTime >= FromDate.Value);

            if (ToDate.HasValue)
                query = query.Where(t => t.DepartureTime <= ToDate.Value);

            Trains = await query.ToListAsync();

            // Подсчёт билетов
            foreach (var train in Trains)
            {
                train.TicketsAvailable = train.Carriages.Sum(c => c.AvailableSeats);
            }

            // Фильтр по наличию свободных мест
            if (OnlyWithAvailableSeats)
                Trains = Trains.Where(t => t.TicketsAvailable > 0).ToList();

            // Итоговые показатели
            TotalTrains = Trains.Count;
            TotalTickets = Trains.Sum(t => t.TicketsAvailable);

            if (Trains.Any())
                AverageTravelTime = TimeSpan.FromMinutes(Trains.Average(t => t.TravelTime.TotalMinutes));
        }
    }
}
