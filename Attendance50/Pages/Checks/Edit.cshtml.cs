using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Attendance50.Data;
using Attendance50.Models;
using Microsoft.AspNetCore.Authorization;

namespace Attendance50.Pages.Checks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _context;

        public EditModel(Attendance50.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["FlowId"] = new SelectList(_context.Flows, "Id", "Name");
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

            _context.Attach(Check).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckExists(Check.Id))
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

        private bool CheckExists(int id)
        {
            return _context.Checks.Any(e => e.Id == id);
        }
    }
}
