using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldTravel.Abstract;
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Dtos.VisaTypes.ViewModels;

namespace WorldTravel.Web.Pages.VisaType
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<VisaTypeViewModel> VisaType { get; set; }

        private readonly IVisaTypeAppService _visaTypeAppService;

        public IndexModel(IVisaTypeAppService visaTypeAppService)
        {
            _visaTypeAppService = visaTypeAppService;
        }
        public async Task<IActionResult> OnGet()
        {
            VisaType = await _visaTypeAppService.GetVisaTypeListForUserAsync(new GetVisaTypeRequestDto());

            return Page();
        }
    }
}
