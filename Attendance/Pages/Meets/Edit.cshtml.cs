using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;

namespace Attendance.Pages.Meets
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Attendance.Data.ApplicationDbContext _context;

        public EditModel(Attendance.Data.ApplicationDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Meet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetExists(Meet.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MeetExists(int id)
        {
            return _context.Meets.Any(e => e.Id == id);
        }
    }
}
