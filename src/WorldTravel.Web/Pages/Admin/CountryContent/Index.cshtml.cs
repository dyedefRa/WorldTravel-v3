using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorldTravel.Web.Pages.Admin.CountryContent
{
    public class IndexModel : PageModel
    {
        public string CountryNameFilter { get; set; }
        public void OnGet()
        {
        }
    }
}
