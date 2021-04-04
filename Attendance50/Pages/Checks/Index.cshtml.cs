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
    public class IndexModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _context;

        public IndexModel(Attendance50.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Check> Check { get;set; }

        public async Task OnGetAsync()
        {
            Check = await _context.Checks
                .Include(c => c.Flow).ToListAsync();
        }
    }
}
