using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Attendance50.Data;
using Attendance50.Models;
using Microsoft.AspNetCore.Authorization;

namespace Attendance50.Pages.Checks
{
    [Authorize]
    public class DeleteCheckModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _db;

        public DeleteCheckModel(Attendance50.Data.ApplicationDbContext context)
        {
            _db = context;
        }
        
        [TempData]
        public int FlowId { get; set; }

        [BindProperty]
        public Check Check { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int flowId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Check = await _db.Checks
                .Include(c => c.Flow).FirstOrDefaultAsync(m => m.Id == id);

            if (Check == null)
            {
                return NotFound();
            }
            FlowId = flowId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Check = await _db.Checks.FindAsync(id);

            if (Check != null)
            {
                _db.Checks.Remove(Check);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("/Flows/Details", new { id = FlowId });
        }
    }
}
