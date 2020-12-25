using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Attendance.Data;
using Attendance.Models;

namespace Attendance.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly Attendance.Data.ApplicationDbContext _context;

        public IndexModel(Attendance.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Student> Student { get;set; }

        public async Task OnGetAsync()
        {
            Student = await _context.Students
                .OrderBy(s => s.Group)
                .ThenBy(s => s.Nick)
                .ToListAsync();
        }
    }
}
