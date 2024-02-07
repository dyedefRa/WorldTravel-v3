using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Volo.Abp.Identity.Web.Pages.Identity;
using WorldTravel.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Components;
using WorldTravel.Enums;
using WorldTravel.Services;
using WorldTravel.Dtos.VisaTypes;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WorldTravel.Web.Pages.Admin.VisaType
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EditModel : WorldTravelPageModel
    {


        [Parameter]
        public int Id { get; set; }

        [BindProperty]
        public EditVisaModel VisaInfo { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public readonly IVisaTypeAppService _visaTypeService;
        private readonly IFileAppService _fileAppService;
        private readonly ILookupAppService _lookupAppService;

        public EditModel(IVisaTypeAppService visaTypeService, IFileAppService fileAppService, ILookupAppService lookupAppService)
        {
            _visaTypeService = visaTypeService;
            _fileAppService = fileAppService;
            _lookupAppService = lookupAppService;
        }

        public async Task OnGetAsync(int id)
        {
            try
            {
                Id = id;
                var visaDto = await _visaTypeService.GetAsync(Id);
                VisaInfo = ObjectMapper.Map<VisaTypeDto, EditVisaModel>(visaDto);
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
                var formGet = await _visaTypeService.GetAsync(Id);
                formGet.Description = VisaInfo.Description;
                formGet.Title = VisaInfo.Title;
                formGet.IsSeenHomePage = VisaInfo.IsSeenHomePage;
                formGet.Rank = VisaInfo.Rank;
                formGet.CountryId = VisaInfo.CountryId;
                formGet.ShortDescription= VisaInfo.ShortDescription;
                formGet.ExtraDescription = VisaInfo.ExtraDescription;
                formGet.VisaDuration= VisaInfo.VisaDuration;
                formGet.VisaFee= VisaInfo.VisaFee;

                if (VisaInfo.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(VisaInfo.MainImage, UploadType.Job);
                    if (fileResult.Success)
                    {
                        formGet.ImageId = fileResult.Data.Id;
                    }
                }

                var input = ObjectMapper.Map<VisaTypeDto, CreateUpdateVisaTypeDto>(formGet);
                await _visaTypeService.UpdateAsync(Id, input);
                return NoContent();
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }

        }

        public class EditVisaModel
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
            public int VisaFee { get; set; }
            public int VisaDuration { get; set; }

            public bool IsSeenHomePage { get; set; }

        }


    }

}
