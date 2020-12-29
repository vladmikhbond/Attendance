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

        [Required]
        public IFormFile UploadedFile { set; get; }      
        [BindProperty]
        public string GroupFilter { set; get; }
        [BindProperty]
        public string MeetComment { set; get; }

        //[BindNever]
        //public Student[] CheckedStudents { set; get; }
        
        [BindNever]
        public IEnumerable<IGrouping<string, Student>> GroupedStudents { set; get; }

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
            if (ModelState.IsValid)
            {
                var checkedStudents = _process.DoCheck(UploadedFile, _db, GroupFilter);

                GroupedStudents = checkedStudents
                    .GroupBy(s => s.Group)
                    .OrderBy(s => s.Key.Replace("-10", "-A"));

                // Do negative Id of students which are absent.
                TempData["checkedIds"] = checkedStudents.Select(s => s.IsPresent ? s.Id : -s.Id).ToArray();
            }
        }

        public IActionResult OnPostSave()
        {
            var checkedIds = (int[])TempData["checkedIds"];
            
            // put new Meet to DB
            var newMeet = new Meet { 
                UserName = User.Identity.Name, 
                When = DateTime.Now, Comment = MeetComment
            };
            _db.Meets.Add(newMeet);
            _db.SaveChanges();

            // put new MeetStudents to DB
            var newMeetStudents = _db.Students.
                Where(s => checkedIds.Contains(s.Id) || checkedIds.Contains(-s.Id))
                .Select(s => new MeetStudent { 
                    StudentId = s.Id, 
                    MeetId = newMeet.Id,
                    IsPresent = checkedIds.Contains(s.Id)
                });

            _db.MeetStudents.AddRange(newMeetStudents);
            _db.SaveChanges();

            return RedirectToPage("Index"); 
        }
    }
}
