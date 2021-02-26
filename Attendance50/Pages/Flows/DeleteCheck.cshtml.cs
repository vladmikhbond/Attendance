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
    public class DeleteCheckModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _context;

        public DeleteCheckModel(Attendance50.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Check Check { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int flowId)
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
            TempData["flowId"] = flowId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Check = await _context.Checks.FindAsync(id);

            if (Check != null)
            {
                _context.Checks.Remove(Check);
                await _context.SaveChangesAsync();
            }
            var flowId = Convert.ToInt32(TempData["flowId"]);
            return RedirectToPage("/Flows/Details", new { id = flowId });
        }
    }
}
