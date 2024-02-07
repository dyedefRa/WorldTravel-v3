using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.Net;
using WorldTravel.Localization;

namespace WorldTravel.Web.Pages.Error
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string ErrorMessage { get; set; }
        private readonly IStringLocalizer<WorldTravelResource> _L;

        public IndexModel(IStringLocalizer<WorldTravelResource> L)
        {
            _L = L;
        }
        public void OnGet(int httpStatusCode)
        {
            if (httpStatusCode == 0 || httpStatusCode == (int)HttpStatusCode.NotFound) //0 || 404
            {
                ErrorMessage = _L["NotFoundPage"];
            }
            else if (httpStatusCode == (int)HttpStatusCode.Unauthorized) //401 YETKISI YOK
            {
                ErrorMessage = _L["UnauthorizedPageB"];
            }
            else
            {
                ErrorMessage = _L["UnexpectedErrorPage"];
            }
        }


    }
}
