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

namespace Attendance50.Pages.Flows
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _db;

        public DeleteModel(Attendance50.Data.ApplicationDbContext context)
        {
            _db = context;
        }

        [BindProperty]
        public Flow Flow { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Flow = await _db.Flows
                .Where(f => f.UserName == User.Identity.Name)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Flow == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Flow = await _db.Flows.FindAsync(id);

            if (Flow != null)
            {
                _db.Flows.Remove(Flow);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
