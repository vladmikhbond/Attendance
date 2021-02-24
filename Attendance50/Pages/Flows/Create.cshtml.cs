using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Attendance50.Data;
using Attendance50.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Attendance50.Pages.Flows
{
    public class CreateModel : PageModel
    {       
        public const string GROUPS_FILTER_NAME = "Groups";
        public MultiSelectList StudentIds;
        public string FilterValue;

        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Flow Flow { get; set; }

        public CreateModel(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult OnGet()
        {
            FilterValue = FilterModel.Value(GROUPS_FILTER_NAME, HttpContext);

            // get filtered students
            var students = 
                from s in _db.Students.Include(s => s.Group)
                where s.Group.Name.StartsWith(FilterValue)
                orderby s.Group.Name, s.Surname
                select new { s.Id, Info = $"{s.Group.Name} {s.Nick}" };

            StudentIds = new MultiSelectList(students, "Id", "Info");
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int[] studentIds )
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            // add new flow
            Flow.UserName = User.Identity.Name;
            _db.Flows.Add(Flow);
            await _db.SaveChangesAsync();

            // add students to flow
            var flowStudents = studentIds.Select(id => new FlowStudent { FlowId = Flow.Id, StudentId = id });
            _db.FlowStudents.AddRange(flowStudents);
            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
