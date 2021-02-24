using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

// Ссылка на фильтр. 

//    <a asp-page="Shared/Filter" asp-route-returnUrl="../Index"
//        asp-route-filterName="@IndexModel.GROUPS_FILTER_NAME" style="margin-left: 40px" title="Filter">
//        Filter
//        <span class="badge-warning" title="Lecture Name Filter">@Model.FilterValue</span>
//    </a>

// Нужно заполнить:
//   asp-route-returnUrl
//   asp-route-filterName
//   <span>


namespace Attendance50.Pages
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

            var url = TempData["returnUrl"].ToString();

            return Redirect(url);

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
