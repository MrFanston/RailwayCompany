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
    public class DetailsModel : PageModel
    {
        private readonly Lab2.Data.RailwayContext _context;

        public DetailsModel(Lab2.Data.RailwayContext context)
        {
            _context = context;
        }

        public Train Train { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains.FirstOrDefaultAsync(m => m.Id == id);
            if (train == null)
            {
                return NotFound();
            }
            else
            {
                Train = train;
            }
            return Page();
        }
    }
}
