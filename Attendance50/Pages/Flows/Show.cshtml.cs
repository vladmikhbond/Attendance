using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Attendance50.Data;
using Attendance50.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Attendance50.Pages.Flows
{
    public class ShowModel : PageModel
    {
        public class ChangeMark
        {
            public int StudentId { set; get; }
            public int CheckId { set; get; }
            public bool WillBePresent { set; get; }
        }

        private readonly ApplicationDbContext _db;

        public ShowModel(ApplicationDbContext context)
        {
            _db = context;
        }

        public Flow Flow { get; set; }
        public Check[] Checks { set; get; }
        public Student[] Students { set; get; }
       

        public async Task<IActionResult> OnGetAsync(int id)
        {            
             Flow = await _db.Flows                
                .SingleOrDefaultAsync(m => m.Id == id);

            Students = (from s in _db.Students
                        from fs in _db.FlowStudents
                        where s.Id == fs.StudentId
                        where fs.FlowId == id  
                        orderby s.Surname
                        select s).ToArray();

            Checks = (from c in _db.Checks
                      where c.FlowId == id
                      orderby c.When
                      select c).Include(c => c.CheckStudents).ToArray();               
            return Page();
        }

    }
}
