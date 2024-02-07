using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Dtos.Jobs.ViewModels;

namespace WorldTravel.Web.Pages.Job
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<JobViewModel> Job { get; set; }

        private readonly IJobAppService _jobAppService;

        public IndexModel(IJobAppService jobAppService)
        {
            _jobAppService = jobAppService;
        }
        public async Task<IActionResult> OnGet()
        {
            Job = await _jobAppService.GetJobListForUserAsync(new GetJobRequestDto());

            return Page();
        }
    }
}
