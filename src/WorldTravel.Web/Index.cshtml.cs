using Microsoft.AspNetCore.Mvc;
using WorldTravel.Web.Pages;

namespace WorldTravel.Web
{
    public class IndexModel : WorldTravelPageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("~/Intro");
        }
    }
}