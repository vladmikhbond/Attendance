using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;

namespace Attendance.Pages.Meets
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Attendance.Data.ApplicationDbContext _context;

        public DeleteModel(Attendance.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Meet Meet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Meet = await _context.Meets.FirstOrDefaultAsync(m => m.Id == id);

            if (Meet == null)
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

            Meet = await _context.Meets.FindAsync(id);

            if (Meet != null)
            {
                _context.Meets.Remove(Meet);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
