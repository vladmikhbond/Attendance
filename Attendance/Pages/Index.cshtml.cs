using Attendance.Data;
using Attendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly Process _process;

        public IFormFile UploadedFile { set; get; }
        [BindProperty]
        public string MeetComment { set; get; }

        [BindNever]
        public Student[] CheckedStaff { set; get; }

        public IndexModel(ApplicationDbContext db, Process process)
        {
            _db = db;
            _process = process;           
        }

        public void OnGet()
        {         
        }

        public void OnPost()
        {
            CheckedStaff = _process.DoCheck(UploadedFile, _db.Students.ToArray());
            TempData["presentIds"] = CheckedStaff.Where(s => s.IsPresent).Select(s => s.Id).ToArray() ;
        }

        public IActionResult OnPostSave()
        {
            var presentIds = (int[])TempData["presentIds"];
            
            // new Meet to DB
            var newMeet = new Meet { 
                UserName = User.Identity.Name, 
                When = DateTime.Now, Comment = MeetComment
            };
            _db.Meets.Add(newMeet);
            _db.SaveChanges();

            // new MeetStudents to DB
            var newMarks = _db.Students.
                Where(s => presentIds.Contains(s.Id))
                .Select(s => new MeetStudent { StudentId = s.Id, MeetId = newMeet.Id });
            _db.MeetStudents.AddRange(newMarks);
            _db.SaveChanges();

            return RedirectToPage("Index"); 
        }
    }
}
