using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin.Job
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CreateModel : WorldTravelPageModel
    {
        [BindProperty]
        public CreateJobModel Job { get; set; }
        public List<SelectListItem> Countries { get; set; }

        private readonly ILookupAppService _lookupAppService;
        private readonly IJobAppService _jobAppService;
        private readonly IFileAppService _fileAppService;

        public CreateModel(
               ILookupAppService lookupAppService,
               IJobAppService jobAppService,
               IFileAppService fileAppService
               )
        {
            _lookupAppService = lookupAppService;
            _jobAppService = jobAppService;
            _fileAppService = fileAppService;
        }

        public async Task OnGet()
        {
            Job = new CreateJobModel();
            Countries = await _lookupAppService.GetCountryLookupAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var createUpdateDto = ObjectMapper.Map<CreateJobModel, CreateUpdateJobDto>(Job);
                List<int> newFileIds = new List<int>();

                if (Job.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(Job.MainImage, UploadType.Job);
                    if (fileResult.Success)
                    {
                        createUpdateDto.ImageId = fileResult.Data.Id;
                    }
                }

                var addedResult = await _jobAppService.CreateAsync(createUpdateDto);
                if (addedResult != null)
                {

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Admin > Job > Create > OnPostAsync");
            }

            return NoContent();
        }

        public class CreateJobModel
        {
            [Required]
            public string Title { get; set; }

            [Required]
            public string HourlyRate { get; set; }

            [Required]
            public string AccommodationFee { get; set; }

            [Required]
            public string LanguageName { get; set; }

            [Required]
            [TextArea(Rows = 2)]
            public string ShortDescription { get; set; }
            [Required]
            [TextArea(Rows = 5)]
            public string Description { get; set; }
            [TextArea(Rows = 5)]
            public string ExtraDescription { get; set; }
            [Required]
            public IFormFile MainImage { get; set; }
            [Required]
            [SelectItems(nameof(Countries))]
            [DisplayName("Country")]
            public int CountryId { get; set; }
            public int Rank { get; set; }
            public bool IsSeenHomePage { get; set; }
        }
    }
}
