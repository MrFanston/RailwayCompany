using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab2.Data;
using Lab2.Models;

namespace Lab2.Pages.Trains
{
    public class IndexModel : PageModel
    {
        private readonly RailwayContext _context;

        public IndexModel(RailwayContext context)
        {
            _context = context;
        }

        public IList<Train> Trains { get;set; } = new List<Train>();

        public async Task OnGetAsync()
        {
            Trains = await _context.Trains.ToListAsync();
        }
    }
}
