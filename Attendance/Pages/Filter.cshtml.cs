using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace Attendance.Pages
{
    public class FilterModel : PageModel
    {
        const string FORMAT = "{0}-filter-{1}";

        [BindProperty(SupportsGet = true)]
        public string FilterName { set; get; }
        [BindProperty(SupportsGet = true)]
        public string FilterValue { set; get; }
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { set; get; }
        //[BindRequired]
        //public StringValues Command { set; get; }

        public void OnGet()
        {
            var cookieName = string.Format(FORMAT, FilterName, User.Identity.Name);
            FilterValue = Request.Cookies[cookieName] ?? "";
            TempData["indiFilterName"] = cookieName;
            TempData["returnUrl"] = ReturnUrl;

        }

        public IActionResult OnPost([FromForm] string command)
        {
            var cookieName = TempData["indiFilterName"].ToString();

            if (command == "Clear")
            {
                Response.Cookies.Delete(cookieName);
            }
            else
            // command is null or "Filter"
            {
                if (string.IsNullOrWhiteSpace(FilterValue))
                {
                    FilterValue = Request.Cookies[cookieName] ?? "";
                }
                Response.Cookies.Append(cookieName, FilterValue, new CookieOptions { Expires = DateTimeOffset.MaxValue });
            }

            var pageName = TempData["returnUrl"].ToString();

            return RedirectToPage(pageName);

        }


        // Get the value of the known filter.
        //
        public static string Value(string filterName, HttpContext http)
        {
            var cookieName = string.Format(FORMAT, filterName, http.User.Identity.Name);
            return http.Request.Cookies[cookieName] ?? "";
        }

    }
}
