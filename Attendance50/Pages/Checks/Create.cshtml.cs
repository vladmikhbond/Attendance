using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Attendance50.Data;
using Attendance50.Models;

namespace Attendance50.Pages.Checks
{
    public class CreateModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _context;

        public CreateModel(Attendance50.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FlowId"] = new SelectList(_context.Flows, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Check Check { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Checks.Add(Check);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
