using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorldTravel.Web.Pages.Admin.VisaType
{
    public class IndexModel : PageModel
    {
        public string VisaTitleFilter { get; set; }

        public void OnGet()
        {
        }
    }
}
