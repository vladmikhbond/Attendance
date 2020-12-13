using Attendance.Models;
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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        public IFormFile UploadedFile { set; get; }

        [BindNever]
        public Student[] Staff { set; get; }
        [BindNever]
        public string When { set; get; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Staff = new Student[0];
        }

        public void OnGet()
        {
           
        }
        public void OnPost([FromServices] Process process, [FromForm] string when)
        {
            Staff = process.DoCheck(UploadedFile);
            When = when;             
        }
    }
}
