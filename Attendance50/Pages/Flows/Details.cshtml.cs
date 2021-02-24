using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Attendance50.Data;
using Attendance50.Models;

namespace Attendance50.Pages.Flows
{
    public class DetailsModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _context;

        public DetailsModel(Attendance50.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Flow Flow { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Flow = await _context.Flows.FirstOrDefaultAsync(m => m.Id == id);

            if (Flow == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
