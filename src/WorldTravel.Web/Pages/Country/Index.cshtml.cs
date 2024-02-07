using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldTravel.Abstract;
using WorldTravel.Dtos.CountryContents;
using WorldTravel.Dtos.CountryContents.ViewModels;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Dtos.Jobs.ViewModels;
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Dtos.VisaTypes.ViewModels;

namespace WorldTravel.Web.Pages.Country
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<CountryContentViewModel> CountryContent { get; set; }
        [BindProperty]
        public List<VisaTypeViewModel> VisaType { get; set; }
        [BindProperty]
        public List<JobViewModel> Job { get; set; }

        private readonly ICountryContentAppService _countryContentAppService;
        private readonly IVisaTypeAppService _visaTypeAppService;
        private readonly IJobAppService _jobAppService;

        public IndexModel(
            ICountryContentAppService countryContentAppService,
            IVisaTypeAppService visaTypeAppService,
            IJobAppService jobAppService
            )
        {
            _countryContentAppService = countryContentAppService;
            _visaTypeAppService = visaTypeAppService;
            _jobAppService = jobAppService;
        }
        public async Task<IActionResult> OnGet()
        {
            CountryContent = await _countryContentAppService.GetCountryContentListForUserAsync(new GetCountryContentRequestDto());
            VisaType = await _visaTypeAppService.GetVisaTypeListForUserAsync(new GetVisaTypeRequestDto() { IsSeenHomePage = true });
            Job = await _jobAppService.GetJobListForUserAsync(new GetJobRequestDto() { IsSeenHomePage = true });
            return Page();
        }
    }
}
