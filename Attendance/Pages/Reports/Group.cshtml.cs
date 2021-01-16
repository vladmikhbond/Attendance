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
    public class GroupModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly Process _process;

        public GroupModel(ApplicationDbContext db, Process process)
        {
            _db = db;
            _process = process;
        }
        [BindProperty]
        public string Group { set; get; }
        //[BindProperty]
        //public MeetStudent MeetStudent { set; get; }

        [BindNever]
        public string[] Groups { set; get; }
        [BindNever]
        public Meet[] Meets { set; get; }
        [BindNever]
        public Student[] Students { set; get; }

        public void OnGet()
        {
            Groups = _db.Students
                .Select(s => s.Group.Name)
                .Distinct()
                .ToArray();

            // Special ordering of the groups
            Groups = Groups.OrderBy(s => s.Replace("-10", "-A")).ToArray();
        }

        public void OnPost()
        {
            OnGet();

            // All students from the selected group
            var students = _db.Students
                .Where(s => s.Group.Name == Group)
                .Include(s => s.MeetStudents)
                .ToArray();

            Students = students.OrderBy(s => s.ReverseName).ToArray();

            // All meets of the User
            Meets = _db.Meets.Where(m => m.UserName == User.Identity.Name)
                .Include(m => m.MeetStudents)
                .OrderBy(m => m.When)
                .ToArray();

            // Filter out not relevant meets
            var groupMeetStudents = Students.SelectMany(s => s.MeetStudents);
            Meets = Meets
                .Where(m => m.MeetStudents.Intersect(groupMeetStudents).Any())
                .ToArray();
        }


        
        public JsonResult OnPostAjax([FromBody] MeetStudentViewModel ms)
        {
            if (ModelState.IsValid)
            {
                var meetStudent = _db.MeetStudents.Find(ms.MeetId, ms.StudentId);
                meetStudent.IsPresent = !meetStudent.IsPresent;
                _db.SaveChanges();
                return new JsonResult(meetStudent);
            }
            return new JsonResult("ERROR");

        }

    }
}
