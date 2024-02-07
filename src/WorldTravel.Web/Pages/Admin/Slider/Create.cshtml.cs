using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using WorldTravel.Dtos.Sliders;
using WorldTravel.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Enums;
using WorldTravel.Services;
using Serilog;

namespace WorldTravel.Web.Pages.Admin.Slider
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CreateModel : WorldTravelPageModel
    {
        [BindProperty]
        public CreateSliderModel SliderInfo { get; set; }


        private readonly ISliderAppService _sliderService;
        private readonly IFileAppService _fileAppService;


        public CreateModel(ISliderAppService sliderService, IFileAppService fileAppService)
        {
            _sliderService = sliderService;
            _fileAppService = fileAppService;
        }

        public void OnGet()
        {
            SliderInfo = new CreateSliderModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var createUpdateDto = ObjectMapper.Map<CreateSliderModel, CreateUpdateSliderDto>(SliderInfo);
                List<int> newFileIds = new List<int>();

                if (SliderInfo.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(SliderInfo.MainImage, UploadType.Job);
                    if (fileResult.Success)
                    {
                        createUpdateDto.ImageId = fileResult.Data.Id;
                    }
                }

                var addedResult = await _sliderService.CreateAsync(createUpdateDto);
                if (addedResult != null)
                {

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Admin > Slider > CreateModal > OnPostAsync");
            }

            return NoContent();
        }

        public class CreateSliderModel
        {
            [Required]
            public string Title { get; set; }

            [Required]
            public string Url { get; set; }
            
            [Required]
            [TextArea(Rows = 2)]
            public string Description { get; set; }
           
            [Required]
            public IFormFile MainImage { get; set; }
         
            public int Rank { get; set; }
            public int IsSeenHomePage { get; set; }

        }
    }
}
