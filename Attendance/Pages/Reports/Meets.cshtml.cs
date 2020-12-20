using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Attendance.Pages.Reports
{
    [Authorize]
    public class MeetsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public MeetsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindNever]
        public Meet[] Meets { set; get; }

        public void OnGet()
        {
            Meets = _db.Meets.Where(m => m.UserName == User.Identity.Name)
                .OrderBy(m => m.When).ToArray();
        }
    }
}
