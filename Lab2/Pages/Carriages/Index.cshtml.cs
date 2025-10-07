using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab2.Data;
using Lab2.Models;

namespace Lab2.Pages.Carriages
{
    public class IndexModel : PageModel
    {
        private readonly Lab2.Data.RailwayContext _context;

        public IndexModel(Lab2.Data.RailwayContext context)
        {
            _context = context;
        }

        public IList<Carriage> Carriage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Carriage = await _context.Carriages
                .Include(c => c.Train).ToListAsync();
        }
    }
}
