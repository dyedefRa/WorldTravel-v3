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
using WorldTravel.Dtos.CountryContents;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin.CountryContent
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CreateModel : WorldTravelPageModel
    {
        [BindProperty]
        public CreateCountryContentModel CountryContent { get; set; }
        public List<SelectListItem> Countries { get; set; }

        private readonly ILookupAppService _lookupAppService;
        private readonly ICountryContentAppService _countryContentAppService;
        private readonly IFileAppService _fileAppService;

        public CreateModel(
               ILookupAppService lookupAppService,
               ICountryContentAppService countryContentAppService,
               IFileAppService fileAppService
               )
        {
            _lookupAppService = lookupAppService;
            _countryContentAppService = countryContentAppService;
            _fileAppService = fileAppService;
        }

        public async Task OnGet()
        {
            CountryContent = new CreateCountryContentModel();
            Countries = await _lookupAppService.GetCountryLookupAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var createUpdateDto = ObjectMapper.Map<CreateCountryContentModel, CreateUpdateCountryContentDto>(CountryContent);
                List<int> newFileIds = new List<int>();

                if (CountryContent.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(CountryContent.MainImage, UploadType.CountryContent);
                    if (fileResult.Success)
                    {
                        createUpdateDto.ImageId = fileResult.Data.Id;
                    }
                }
                if (CountryContent.ImportFiles != null)
                {
                    foreach (var file in CountryContent.ImportFiles)
                    {
                        var fileResult = await _fileAppService.SaveFileAsync(file, UploadType.CountryContent);
                        if (fileResult.Success)
                        {
                            newFileIds.Add(fileResult.Data.Id);
                        }
                    }
                }

                var addedResult = await _countryContentAppService.CreateAsync(createUpdateDto);
                if (addedResult != null)
                {
                    await _countryContentAppService.AddOrRemoveCountryContentFileRelationAsync(addedResult.Id, newFileIds);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Admin > CountryContent > Create > OnPostAsync");
            }

            return NoContent();
        }

        public class CreateCountryContentModel
        {
            public CreateCountryContentModel()
            {
                ImportFiles = new List<IFormFile>();
            }
            [Required]
            public string Title { get; set; }
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
            public List<IFormFile> ImportFiles { get; set; }
        }
    }
}
