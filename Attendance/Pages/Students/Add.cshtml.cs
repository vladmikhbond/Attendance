using System;
using System.Collections.Generic;
using System.Linq;
using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Attendance.Pages
{
    [Authorize]
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
            // 0-name  1-surname   2-group name
            var ree = StringSplitOptions.RemoveEmptyEntries;
            var table = StudentList
                .Split(new char[] { '\n', '\r' }, ree)
                .Select(s => s.Split('\t'));

            int lineNo = 1;
            foreach (var a in table)
            {
                if (a.Length != 3)
                    ModelState.AddModelError("StudentList", $"ERROR in line {lineNo}");
                lineNo++;
            }
            if (!ModelState.IsValid)
                return null;

            // add new groups
            var oldGroupNames = _db.Groups.Select(g => g.Name).ToArray();

            var newGroups = table
                .Select(a => a[2])
                .Distinct()
                .Except(oldGroupNames)
                .Select(n => new Group { Name = n });

            _db.Groups.AddRange(newGroups);
            _db.SaveChanges();



            // add new students
            var oldTokens = _db.Students.ToArray()
                .Select(s => $"{s.GroupId} {s.Nick}");

            var newStudents = table
                .Select(a => new Student
                {
                    Nick = $"{a[0]} {a[1]}".Trim(),
                    GroupId = _db.Groups.Single(g => g.Name == a[2]).Id
                })
                .Where(s => !oldTokens.Contains($"{s.GroupId} {s.Nick}"));

            _db.Students.AddRange(newStudents);
            _db.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}
