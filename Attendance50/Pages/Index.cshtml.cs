using Attendance50.Data;
using Attendance50.Models;
using Attendance50.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance50.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {


        private readonly ApplicationDbContext _db;
        private readonly Process _process;

        [Required]
        public IFormFile UploadedFile { set; get; }      
        [BindProperty]
        public string MeetComment { set; get; }     

        [BindNever]
        public IEnumerable<IGrouping<string, Student>> GroupedStudents { set; get; }
        [BindNever]
        public SelectList FlowSelectList { set; get; }

        public IndexModel(ApplicationDbContext db, Process process)
        {
            _db = db;
            _process = process;           
        }

        public void OnGet()
        {
            var flows = _db.Flows.Where(f => f.UserName == User.Identity.Name);
            FlowSelectList = new SelectList(flows, "Id", "Name");

        }

        
        public IActionResult OnPost(int[] flowIds)
        {
            if (flowIds.Length == 0) {
                ModelState.AddModelError("", "Select Flow");                   
            }
            if (ModelState.IsValid)
            {
                var flowId = flowIds[0];
                var allStudents = (from s in _db.Students
                        from fs in _db.FlowStudents
                        where fs.FlowId == flowId
                        where s.Id == fs.StudentId
                        select s).ToArray();

                var presentsNames = _process.GetPresentNames(UploadedFile);

                var presentStudents = allStudents
                     .Where(s => presentsNames.Contains(s.Nick))
                     .ToArray();

                // Create new Check and save fo DB 

                var newCheck = new Check
                {       
                    FlowId = flowId,
                    When = DateTime.Now,
                };
                newCheck.CheckStudents = presentStudents.Select(s => new CheckStudent { StudentId = s.Id }).ToList();

                _db.SaveChanges();

                // return to report
                return Content(allStudents.Count().ToString());
            }

            var flows = _db.Flows.Where(f => f.UserName == User.Identity.Name);
            FlowSelectList = new SelectList(flows, "Id", "Name");

            return Page();


        }

    }
}
