using System;
using System.Collections.Generic;
using System.Linq;
using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Attendance.Pages
{
    public class NewStudentsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public string StudentList { set; get;}

        public NewStudentsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            RedirectToPage("Index");
        }

        public IActionResult OnPost()
        {
            var allStuds = _db.Students.ToArray();
            
            var studLines = StudentList.Split(new char[]{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            var newStuds = new List<Student>();

            var errorMessge = "";
            foreach (var studLine in studLines)
            {
                var ss = studLine.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length != 2 && ss.Length != 3)
                {
                    errorMessge += $"Wrong data: '{studLine}'\n";
                    continue;
                }
                var newStud = ss.Length == 3 ?
                    new Student { Nick = ss[0] + " " + ss[1], Group = ss[2] } :
                    new Student { Nick = ss[0], Group = ss[1] };

                if (allStuds.Contains(newStud, new StudentComparer()))
                {
                    errorMessge += $"Not unique: '{studLine}'\n";
                    continue;
                }
                newStuds.Add(newStud);

            }
            if (!string.IsNullOrEmpty(errorMessge))
            {
                ModelState.AddModelError("StudentList", errorMessge);
            }
            
            if (ModelState.IsValid)
            {
                _db.Students.AddRange(newStuds);
                _db.SaveChanges();
                return RedirectToPage("Index");
            }
            return null;
        }
    }
}
