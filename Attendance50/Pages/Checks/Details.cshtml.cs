using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Attendance50.Data;
using Attendance50.Models;

namespace Attendance50.Pages.Checks
{
    public class DetailsModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _context;

        public DetailsModel(Attendance50.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Check Check { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Check = await _context.Checks
                .Include(c => c.Flow).FirstOrDefaultAsync(m => m.Id == id);

            if (Check == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
