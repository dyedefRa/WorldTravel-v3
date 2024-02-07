using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Volo.Abp.Identity.Web.Pages.Identity;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Sliders.ViewModels;
using WorldTravel.Dtos.Sliders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Components;
using WorldTravel.Enums;
using WorldTravel.Services;
using WorldTravel.Dtos.CountryContents;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;

namespace WorldTravel.Web.Pages.Admin.CountryContent
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EditModel : WorldTravelPageModel
    {


        [Parameter]
        public int Id { get; set; }

        [BindProperty]
        public EditCountryContentModel CountryContentInfo { get; set; }
        public List<SelectListItem> Countries { get; set; }


        public readonly ICountryContentAppService _countryContentService;
        private readonly IFileAppService _fileAppService;
        private readonly ILookupAppService _lookupAppService;

        public EditModel(ICountryContentAppService countryContentService, IFileAppService fileAppService, ILookupAppService lookupAppService)
        {
            _countryContentService = countryContentService;
            _fileAppService = fileAppService;
            _lookupAppService = lookupAppService;
        }

        public async Task OnGetAsync(int id)
        {
            try
            {
                Id = id;
                var countryContentDto = await _countryContentService.GetAsync(Id);
                CountryContentInfo = ObjectMapper.Map<CountryContentDto, EditCountryContentModel>(countryContentDto);
                Countries = await _lookupAppService.GetCountryLookupAsync();
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
                var formGet = await _countryContentService.GetAsync(Id);
                formGet.Description = CountryContentInfo.Description;
                formGet.Title = CountryContentInfo.Title;
                formGet.IsSeenHomePage = CountryContentInfo.IsSeenHomePage;
                formGet.Rank = CountryContentInfo.Rank;
                formGet.CountryId = CountryContentInfo.CountryId;
                formGet.ShortDescription = CountryContentInfo.ShortDescription;
                formGet.ExtraDescription = CountryContentInfo.ExtraDescription;
                if (CountryContentInfo.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(CountryContentInfo.MainImage, UploadType.Job);
                    if (fileResult.Success)
                    {
                        formGet.ImageId = fileResult.Data.Id;
                    }
                }

                var input = ObjectMapper.Map<CountryContentDto, CreateUpdateCountryContentDto>(formGet);
                await _countryContentService.UpdateAsync(Id, input);
                return NoContent();
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }

        }

        public class EditCountryContentModel
        {
            [HiddenInput]
            public int Id { get; set; }

            public string Title { get; set; }
           
            [TextArea(Rows = 2)]
            public string ShortDescription { get; set; }
          
            [TextArea(Rows = 5)]
            public string Description { get; set; }
            [TextArea(Rows = 5)]
            public string ExtraDescription { get; set; }
            public IFormFile MainImage { get; set; }
          
            [SelectItems(nameof(Countries))]
            [DisplayName("Country")]
            public int CountryId { get; set; }
            public int Rank { get; set; }
            public bool IsSeenHomePage { get; set; }

        }


    }

}
