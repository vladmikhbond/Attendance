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

        [BindNever]
        public Meet[] Meets { set; get; }
        [BindNever]
        public Student[] CheckedSudents { set; get; }

        public void OnGet()
        {
            Meets = _db.Meets.Where(m => m.UserName == User.Identity.Name)
                .OrderBy(m => m.When).ToArray();
        }

        public void OnPost(int id)
        {
            Meets = _db.Meets.Where(m => m.UserName == User.Identity.Name)
                .OrderBy(m => m.When).ToArray();

            var checkeds = _db.MeetStudents
                .Where(ms => ms.MeetId == id)
                .Include(ms => ms.Student)     // using Microsoft.EntityFrameworkCore;                
                .Select(ms => new { Student = ms.Student, isPresent = ms.IsPresent })

                .ToArray();

            foreach (var c in checkeds)
            {
                c.Student.IsPresent = c.isPresent;
            }

            CheckedSudents = checkeds.Select(c => c.Student)
                .OrderBy(s => s.Group)
                .ThenBy(s => s.Surname)
                .ToArray();
        }
    }
}