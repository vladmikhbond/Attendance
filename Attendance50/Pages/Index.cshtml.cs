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
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            // ------ Inner func
            IActionResult Error(string message)
            {
                ModelState.AddModelError("UploadedFile", message);
                var flows = _db.Flows.Where(f => f.UserName == User.Identity.Name);
                FlowSelectList = new SelectList(flows, "Id", "Name");
                return Page();
            }
            // ------

            var presentsNicks = _process.GetPresentNames(UploadedFile);
            if (presentsNicks.Length == 0)
            {
                return Error("No students found");
            }

            // Define flow id
            int flowId = flowIds.Length > 0 ? flowIds[0] : 0;

            
            if (flowId == 0)
            {
                // Flow is not selected. Define it by default
                flowId = (from s in _db.Students
                        from fs in _db.FlowStudents
                        from f in _db.Flows
                        where f.Id == fs.FlowId
                        where s.Id == fs.StudentId
                        where f.UserName == User.Identity.Name
                        where presentsNicks.Contains(s.Nick)
                        group fs by fs.FlowId into g
                        select new { FlowId = g.Key, Count = g.Count() } into p
                        orderby p.Count
                        select p.FlowId).LastOrDefault();
            }
            if (flowId == 0)
            {
                return Error("Can not define a flow.");
            } 

            var allStudents = (from s in _db.Students
                    from fs in _db.FlowStudents
                    where fs.FlowId == flowId
                    where s.Id == fs.StudentId
                    select s).ToArray();
                
            var presentStudents = allStudents
                    .Where(s => presentsNicks.Contains(s.Nick))
                    .ToArray();

            // Create new check and save fo DB 

            var newCheck = new Check
            {
                FlowId = flowId,
                When = DateTime.Now,
                Raw = string.Join('\t', presentsNicks)
            }; 
            

            newCheck.CheckStudents = presentStudents.Select(s => new CheckStudent { StudentId = s.Id }).ToList();
            _db.Checks.Add(newCheck);
            _db.SaveChanges();

            // return to report
            return Redirect($"Flows/Details?id={flowId}");
        }

    }
}
