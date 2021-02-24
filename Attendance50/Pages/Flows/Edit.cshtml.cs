using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Attendance50.Data;
using Attendance50.Models;

namespace Attendance50.Pages.Flows
{
    public class EditModel : PageModel
    {
        public class StudInfo
        {
            public int Id { set; get; }
            public string Info { set; get; }
            public bool IsSelected;
        }

        public IEnumerable<StudInfo> AllStudents { set; get; }        
        

        public string FilterValue;
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Flow Flow { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Flow = await _context.Flows.Include(f => f.FlowStudents).FirstOrDefaultAsync(m => m.Id == id);

            if (Flow == null)
            {
                return NotFound();
            }

            FilterValue = FilterModel.Value(CreateModel.GROUPS_FILTER_NAME, HttpContext);

            int[] selStudentIds = Flow.FlowStudents
                .Select(fs => fs.StudentId).ToArray();

            AllStudents =
                from s in _context.Students.Include(s => s.Group)
                where s.Group.Name.StartsWith(FilterValue)
                orderby s.Group.Name, s.Surname
                select new StudInfo {
                    Id = s.Id,
                    Info = $"{s.Group.Name} {s.Nick}",
                    IsSelected = selStudentIds.Contains(s.Id)
                };
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int[] studentIds)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var flow = _context
                .Flows.Include(f => f.FlowStudents)
                .SingleOrDefault(f => f.Id == Flow.Id);
            
            flow.FlowStudents = studentIds
                .Select(id => new FlowStudent { FlowId = Flow.Id,  StudentId = id })
                .ToList();




            //_context.Attach(Flow).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowExists(Flow.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FlowExists(int id)
        {
            return _context.Flows.Any(e => e.Id == id);
        }
    }
}
