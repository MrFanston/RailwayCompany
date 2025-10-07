using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Pages.Reports
{
    public class CarriageReportModel : PageModel
    {
        private readonly RailwayContext _context;

        public CarriageReportModel(RailwayContext context)
        {
            _context = context;
        }

        // �������
        [BindProperty(SupportsGet = true)]
        public int? TrainId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Type { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool OnlyWithAvailableSeats { get; set; }

        // ����������
        public IList<Carriage> Carriages { get; set; } = new List<Carriage>();
        public IList<Train> Trains { get; set; } = new List<Train>();

        // �����
        public int TotalSeats => Carriages.Sum(c => c.SeatCount);
        public int AvailableSeats => Carriages.Sum(c => c.AvailableSeats);

        public async Task OnGetAsync()
        {
            // ��������� ��� ������ � �������� � ������
            var query = await _context.Carriages
                .Include(c => c.Train)
                .ToListAsync();

            // ��������� ������� � ������
            if (TrainId.HasValue)
                query = query.Where(c => c.TrainId == TrainId.Value).ToList();

            if (!string.IsNullOrEmpty(Type))
                query = query.Where(c => c.Type == Type).ToList();

            if (OnlyWithAvailableSeats)
                query = query.Where(c => c.AvailableSeats > 0).ToList();

            // ���������� � ������
            Carriages = query
                .OrderBy(c => c.Train.TrainNumber)
                .ThenBy(c => c.CarriageNumber)
                .ToList();

            // ������ ������� ��� �������
            Trains = (await _context.Trains.ToListAsync())
                .OrderBy(t => t.TrainNumber)
                .ToList();
        }
    }
}
