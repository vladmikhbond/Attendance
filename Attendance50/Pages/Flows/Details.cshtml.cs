﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Attendance50.Data;
using Attendance50.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;

namespace Attendance50.Pages.Flows
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        public class ChangeMark
        {
            public int StudentId { set; get; }
            public int CheckId { set; get; }
            public bool WillBePresent { set; get; }
        }

        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext context)
        {
            _db = context;
        }

        public Flow Flow { get; set; }
        public Check[] Checks { set; get; }
        public Student[] Students { set; get; }
       

        public async Task<IActionResult> OnGetAsync(int id)
        {
             string u = User.Identity.Name;
             Flow = await _db.Flows
                .Where(f => u == f.UserName || u == "opr")  // Easter egg
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

        public JsonResult OnPostAjax( [FromBody] ChangeMark changeMark)
        {
            try {
                var checkStudent = new CheckStudent { CheckId = changeMark.CheckId, StudentId = changeMark.StudentId };

                if (changeMark.WillBePresent)
                {
                    _db.CheckStudents.Add(checkStudent);
                }
                else
                {
                    _db.CheckStudents.Remove(checkStudent);
                }
                _db.SaveChanges();
                return new JsonResult(changeMark);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}
