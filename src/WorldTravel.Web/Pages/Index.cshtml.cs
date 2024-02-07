using Microsoft.AspNetCore.Mvc;

namespace WorldTravel.Web.Pages
{
    public class IndexModel : WorldTravelPageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("~/Intro");
        }
    }
}