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
    public class IndexModel : PageModel
    {
        private readonly Attendance.Data.ApplicationDbContext _context;

        public IndexModel(Attendance.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Meet> Meet { get;set; }

        public async Task OnGetAsync()
        {
            Meet = await _context.Meets
                .Where(m => m.UserName == User.Identity.Name)
                .ToListAsync();
        }
    }
}
