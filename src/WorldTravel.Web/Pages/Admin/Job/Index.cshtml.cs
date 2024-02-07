using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorldTravel.Web.Pages.Admin.Job
{
    public class IndexModel : PageModel
    {
        public string JobTitleFilter { get; set; }

        public void OnGet()
        {
        }
    }
}
