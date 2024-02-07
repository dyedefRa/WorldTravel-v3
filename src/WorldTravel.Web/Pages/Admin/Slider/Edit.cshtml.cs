using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Sliders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Microsoft.AspNetCore.Components;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin.Slider
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EditModel : WorldTravelPageModel
    {

        [Parameter]
        public int Id { get; set; }

        [BindProperty]
        public EditSliderModel SliderInfo { get; set; }

        private readonly ISliderAppService _sliderService;
        private readonly IFileAppService _fileAppService;

        public EditModel(ISliderAppService sliderService, IFileAppService fileAppService)
        {
            _sliderService = sliderService;
            _fileAppService = fileAppService;
        }

        public async Task OnGetAsync(int id)
        {
            try
            {
                Id = id;
                var sliderDto = await _sliderService.GetAsync(Id);
                SliderInfo = ObjectMapper.Map<SliderDto, EditSliderModel>(sliderDto);
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }

        }

        public virtual async Task<IActionResult> OnPostAsync(int Id)
        {
            try
            {
                ValidateModel();
                var formGet = await _sliderService.GetAsync(Id);
                formGet.Description = SliderInfo.Description;
                formGet.Title = SliderInfo.Title;
                formGet.IsSeenHomePage = SliderInfo.IsSeenHomePage;
                formGet.Url = SliderInfo.Url;
                formGet.Rank = SliderInfo.Rank;

                if (SliderInfo.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(SliderInfo.MainImage, UploadType.Job);
                    if (fileResult.Success)
                    {
                        formGet.ImageId = fileResult.Data.Id;
                    }
                }

                var input = ObjectMapper.Map<SliderDto, CreateUpdateSliderDto>(formGet);
                await _sliderService.UpdateAsync(Id, input);
                return NoContent();
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }

        }

        public class EditSliderModel
        {
            [HiddenInput]
            public int Id { get; set; }

            public string Title { get; set; }

            public string Url { get; set; }

            [TextArea(Rows = 2)]
            public string Description { get; set; }

            public IFormFile MainImage { get; set; }

            public int Rank { get; set; }
            public bool IsSeenHomePage { get; set; }

        }
    }

}
