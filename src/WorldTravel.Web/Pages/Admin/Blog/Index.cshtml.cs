using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorldTravel.Web.Pages.Admin.Blog
{
    public class IndexModel : PageModel
    {
        public string BlogTitleFilter { get; set; }

        public void OnGet()
        {
        }
    }
}
