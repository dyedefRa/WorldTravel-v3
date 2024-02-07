using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorldTravel.Web.Pages.Admin.Slider
{
    public class IndexModel : PageModel
    {
        public string SliderTitleFilter { get; set; }

        public void OnGet()
        {
        }
    }
}
