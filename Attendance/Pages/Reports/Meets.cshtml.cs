using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.EntityFrameworkCore;


namespace Attendance.Pages.Reports
{
    [Authorize]
    public class MeetsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly Process _process;

        public MeetsModel(ApplicationDbContext db, Process process)
        {
            _db = db;
            _process = process;
        }

        [BindProperty]
        public string GroupFilter { set; get; }

        [BindNever]
        public Meet[] Meets { set; get; }
        [BindNever]
        public Student[] Sudents { set; get; }

        public void OnGet()
        {
            Meets = _db.Meets.Where(m => m.UserName == User.Identity.Name)
                .Include(m => m.MeetStudents)
                .OrderBy(m => m.When)
                .ToArray();

            var studIs = Meets
                .SelectMany(m => m.MeetStudents)
                .Select(ms => ms.StudentId)
                .Distinct();

            Sudents = _db.Students
                .Where(s => studIs.Contains(s.Id))
                .OrderBy(s => s.Group)
                .ThenBy(s => s.Surname)
                .ToArray();

            if (!string.IsNullOrWhiteSpace(GroupFilter))
            {
                Regex regex = new(GroupFilter);
                Sudents = Sudents.Where(s => regex.IsMatch(s.Group)).ToArray();
            }     
        }

        public void OnPost()
        {
            OnGet();
        }
    }
}
