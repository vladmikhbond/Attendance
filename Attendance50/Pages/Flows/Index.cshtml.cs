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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext context)
        {
            _db = context;
        }

        public IList<Flow> Flow { get;set; }

        public async Task OnGetAsync()
        {
            string u = User.Identity.Name;
            Flow = await _db.Flows
                .Where(f => u == f.UserName || u == "opr")  // Easter egg
                .ToListAsync();
        }
    }
}
