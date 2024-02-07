using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using WorldTravel.Abstract;
using WorldTravel.Dtos.CountryContents;
using WorldTravel.Enums;
using Serilog;
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Services;

namespace WorldTravel.Web.Pages.Admin.VisaType
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CreateModel : WorldTravelPageModel
    {
        [BindProperty]
        public CreateVisaTypeModel VisaType { get; set; }
        public List<SelectListItem> Countries { get; set; }

        private readonly ILookupAppService _lookupAppService;
        private readonly IVisaTypeAppService _visaTypeAppService;
        private readonly IFileAppService _fileAppService;

        public CreateModel(
               ILookupAppService lookupAppService,
               IVisaTypeAppService visaTypeAppService,
               IFileAppService fileAppService
               )
        {
            _lookupAppService = lookupAppService;
            _visaTypeAppService = visaTypeAppService;
            _fileAppService = fileAppService;
        }

        public async Task OnGet()
        {
            VisaType = new CreateVisaTypeModel();
            Countries = await _lookupAppService.GetCountryLookupAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var createUpdateDto = ObjectMapper.Map<CreateVisaTypeModel, CreateUpdateVisaTypeDto>(VisaType);
                List<int> newFileIds = new List<int>();

                if (VisaType.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(VisaType.MainImage, UploadType.VisaType);
                    if (fileResult.Success)
                    {
                        createUpdateDto.ImageId = fileResult.Data.Id;
                    }
                }

                var addedResult = await _visaTypeAppService.CreateAsync(createUpdateDto);
                if (addedResult != null)
                {

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Admin > VisaType > Create > OnPostAsync");
            }

            return NoContent();
        }

        public class CreateVisaTypeModel
        {
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
            public int VisaFee { get; set; }
            public int VisaDuration { get; set; }
            public bool IsSeenHomePage { get; set; }
        }
    }
}
