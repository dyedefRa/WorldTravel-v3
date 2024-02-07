using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Volo.Abp.Identity.Web.Pages.Identity;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Jobs.ViewModels;
using WorldTravel.Dtos.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Components;
using WorldTravel.Enums;
using WorldTravel.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;

namespace WorldTravel.Web.Pages.Admin.Job
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EditModel : WorldTravelPageModel
    {

        [Parameter]
        public int Id { get; set; }

        [BindProperty]
        public EditJobModel JobInfo { get; set; }

        public List<SelectListItem> Countries { get; set; }


        public readonly IJobAppService _jobService;
        private readonly IFileAppService _fileAppService;
        private readonly ILookupAppService _lookupAppService;

        public EditModel(IJobAppService jobService, IFileAppService fileAppService, ILookupAppService lookupAppService)
        {
            _jobService = jobService;
            _fileAppService = fileAppService;
            _lookupAppService = lookupAppService;
        }

        public async Task OnGetAsync(int id)
        {
            try
            {
                Id = id;
                var JobDto = await _jobService.GetAsync(Id);
                JobInfo = ObjectMapper.Map<JobDto, EditJobModel>(JobDto);
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
                var formGet = await _jobService.GetAsync(Id);
                formGet.Description = JobInfo.Description;
                formGet.Title = JobInfo.Title;
                formGet.IsSeenHomePage = JobInfo.IsSeenHomePage;
                formGet.Rank = JobInfo.Rank;
                formGet.CountryId = JobInfo.CountryId;
                formGet.ShortDescription = JobInfo.ShortDescription;
                formGet.ExtraDescription = JobInfo.ExtraDescription;
                formGet.HourlyRate = JobInfo.HourlyRate;
                formGet.AccommodationFee = JobInfo.AccommodationFee;
                formGet.LanguageName = JobInfo.LanguageName;
                if (JobInfo.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(JobInfo.MainImage, UploadType.Job);
                    if (fileResult.Success)
                    {
                        formGet.ImageId = fileResult.Data.Id;
                    }
                }

                var input = ObjectMapper.Map<JobDto, CreateUpdateJobDto>(formGet);
                await _jobService.UpdateAsync(Id, input);
                return NoContent();
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }

        }

        public class EditJobModel
        {
            [HiddenInput]
            public int Id { get; set; }

            public string Title { get; set; }
            public string HourlyRate { get; set; }
            public string AccommodationFee { get; set; }
            public string LanguageName { get; set; }

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
