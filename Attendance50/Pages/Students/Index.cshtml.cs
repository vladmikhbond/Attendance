using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Attendance50.Data;
using Attendance50.Models;

namespace Attendance50.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly Attendance50.Data.ApplicationDbContext _context;

        public IndexModel(Attendance50.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Student> Students { get;set; }

        public void OnGetAsync()
        {
            Students = (from s in _context.Students.Include(s => s.Group)
                       orderby s.Group.Name, s.Surname
                       select s).ToList();
                
        }
    }
}
