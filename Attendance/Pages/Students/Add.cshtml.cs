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

            var errorMes = "";
            foreach (var studLine in studLines)
            {
                var ss = studLine.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length < 5)
                {
                    errorMes += $"Wrong data: '{studLine}'\n";
                    continue;
                }
                var newStud = new Student { Name = ss[0], Surname = ss[1], Patronim = ss[2], Email = ss[3], Group = ss[4] };
                if (allStuds.Contains(newStud, new StudentComparer()))
                {
                    errorMes += $"Not unique: '{studLine}'\n";
                    continue;
                }
                newStuds.Add(newStud);

            }
            if (!string.IsNullOrEmpty(errorMes))
            {
                ModelState.AddModelError("StudentList", errorMes);
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
