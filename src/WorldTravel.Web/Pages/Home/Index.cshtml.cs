using Microsoft.AspNetCore.Identity;
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
using WorldTravel.Dtos.Blog;
using WorldTravel.Dtos.Blog.ViewModels;
using WorldTravel.Dtos.CountryContents.ViewModels;
using WorldTravel.Dtos.Forms;
using WorldTravel.Dtos.Jobs.ViewModels;
using WorldTravel.Dtos.Sliders;
using WorldTravel.Dtos.Sliders.ViewModels;
using WorldTravel.Dtos.VisaTypes.ViewModels;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Home
{
    [AutoValidateAntiforgeryToken]
    public class IndexModel : WorldTravelPageModel
    {
        [BindProperty]
        public FormModel FormInputModel { get; set; }
        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<CountryContentViewModel> CountryContent { get; set; }
        public List<VisaTypeViewModel> VisaType { get; set; }
        public List<JobViewModel> Job { get; set; }
        public List<BlogViewModel> Blog { get; set; }
        public List<SliderViewModel> Slider { get; set; }

        private readonly ILookupAppService _lookupAppService;
        private readonly IFormAppService _formAppService;
        private readonly ICountryContentAppService _countryContentAppService;
        private readonly IVisaTypeAppService _visaTypeAppService;
        private readonly IJobAppService _jobAppService;
        private readonly IBlogAppService _blogAppService;
        private readonly ISliderAppService _sliderAppService;
        private readonly IFileAppService _fileAppService;
        private readonly IUserAppService _userAppService;
        private SignInManager<Volo.Abp.Identity.IdentityUser> _signInManager;


        public IndexModel(
               ILookupAppService lookupAppService,
               IFormAppService formAppService,
               ICountryContentAppService countryContentAppService,
               IVisaTypeAppService visaTypeAppService,
               IJobAppService jobAppService,
               IBlogAppService blogAppService,
               IFileAppService fileAppService,
               ISliderAppService sliderAppService,
               IUserAppService userAppService,
               SignInManager<Volo.Abp.Identity.IdentityUser> signInManager
               )
        {
            _lookupAppService = lookupAppService;
            _formAppService = formAppService;
            _sliderAppService = sliderAppService;
            _countryContentAppService = countryContentAppService;
            _visaTypeAppService = visaTypeAppService;
            _jobAppService = jobAppService;
            _fileAppService = fileAppService;
            _blogAppService = blogAppService;
            _userAppService = userAppService;
            _signInManager = signInManager;
        }

        private async Task LoadInitializeData()
        {
            FormInputModel = new FormModel();
            Genders = _lookupAppService.GetGenderLookup();
            Countries = await _lookupAppService.GetCountryLookupAsync();
            CountryContent = await _countryContentAppService.GetCountryContentListForUserAsync(
                new Dtos.CountryContents.GetCountryContentRequestDto() { IsSeenHomePage = true });
            VisaType = await _visaTypeAppService.GetVisaTypeListForUserAsync(
                new Dtos.VisaTypes.GetVisaTypeRequestDto() { IsSeenHomePage = true });
            Job = await _jobAppService.GetJobListForUserAsync(
                new Dtos.Jobs.GetJobRequestDto() { IsSeenHomePage = true });
            Blog = await _blogAppService.GetBlogListForAsync(new GetBlogRequestDto() { IsSeenHomePage = true });
            Slider = await _sliderAppService.GetSliderListHomeAsync(new GetSliderRequestDto() { IsSeenHomePage = true });
        }

        public async Task<IActionResult> OnGet(bool r = false)
        {
            try
            {
                if (CurrentUser.IsAuthenticated)
                {
                    //if (CurrentUser.Name == "admin")
                    //{
                    //    await _signInManager.SignOutAsync();
                    //    return Redirect("~/Account/Login");
                    //}

                    var currentUser = await _userAppService.GetAppUserAsync(CurrentUser.Id.Value);
                    if (currentUser == null || currentUser.Data == null || currentUser.Data.Status != Status.Active /*|| currentUser.Data.UserType != UserType.User*/)
                    {
                        return Redirect("~/Account/Login");
                    }
                }
            }
            catch (Exception)
            {
                return Redirect("~/Account/Login");
            }

            await LoadInitializeData();
            if (r)
            {
                Alerts.Info(L["FormRequestSuccessfully"].Value);
                FormInputModel.IsReturnPost = true;
            }

            TempData["surec"] = true;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (CurrentUser.UserName != null)
                {
                    if (ModelState.IsValid)
                    {
                        FormInputModel.UserId = CurrentUser.Id;
                        FormInputModel.FormIsOk = 0;

                        FormInputModel.JobTip = "yok";
                        FormInputModel.JobPosition = "yok";
                        FormInputModel.PositionName = "yok";
                        FormInputModel.Sector = "yok";
                        FormInputModel.JobTitle = "yok";
                        FormInputModel.CompanyContact = "yok";
                        FormInputModel.CompanyName = "yok";
                        FormInputModel.EducationStatus = "yok";
                        FormInputModel.RequiredQualification = "yok";
                        FormInputModel.SpecialRequest = "yok";
                        FormInputModel.IsNeedConsult = "yok";
                        FormInputModel.IsBecomeConsultDesc = "yok";
                        FormInputModel.HoursWork = "yok";
                        FormInputModel.SalaryRange = "yok";
                        FormInputModel.Profession = "yok";
                        FormInputModel.Location = "yok";

                        var form = new FormDto
                        {
                            FormRegType = "Vize Başvurusu",
                            SalaryRange = FormInputModel.SalaryRange,
                            BirthDate = DateTime.Now,
                            Sector = FormInputModel.Sector,
                            SpecialRequest = FormInputModel.SpecialRequest,
                            CompanyContact = FormInputModel.CompanyContact,
                            CompanyName = FormInputModel.CompanyName,
                            RequiredQualification = FormInputModel.RequiredQualification,
                            CountryId = FormInputModel.CountryId,
                            CreatedDate = DateTime.Now,
                            Description = FormInputModel.Description != null || FormInputModel.Description != "" ? FormInputModel.Description : "yok",
                            EducationStatus = FormInputModel.EducationStatus,
                            Email = FormInputModel.Email,
                            FormIsOk = 0,
                            Gender = FormInputModel.Gender,
                            HoursWork = FormInputModel.HoursWork,
                            IsBecomeConsultDesc = FormInputModel.IsBecomeConsultDesc,
                            IsContacted = false,
                            IsNeedConsult = FormInputModel.IsNeedConsult,
                            JobPosition = FormInputModel.JobPosition,
                            JobTip = FormInputModel.JobTip,
                            JobTitle = FormInputModel.JobTitle,
                            Location = FormInputModel.Location,
                            Name = FormInputModel.Name,
                            PhoneNumber = FormInputModel.PhoneNumber,
                            PositionName = FormInputModel.PositionName,
                            Profession = FormInputModel.Profession,
                            Surname = FormInputModel.Surname,
                            UserId = (Guid)CurrentUser.Id,
                            Status = Status.Active,
                        };

                        //var input = ObjectMapper.Map<FormDto, CreateUpdateFormDto>(form);
                        var addedResult = await _formAppService.CreateManualAsync(form);
                        if (addedResult != null)
                        {
                            return RedirectToAction("Manage", "Account", new { returnPost = true });
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Home > IndexModel > OnPostAsync has error! ");
            }

            Alerts.Danger(L["GeneralError"].Value);
            return Page();
        }

        public class FormModel
        {

            public FormModel()
            {
            }

            [Required]
            public string Name { get; set; }
            [Required]
            public string Surname { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [StringLength(16, MinimumLength = 16, ErrorMessage = "Telefon alanı geçerli bir telefon numarası olmalıdır.")]
            public string PhoneNumber { get; set; }
            [Required]
            [SelectItems(nameof(Gender))]
            public GenderType Gender { get; set; }
            public DateTime? BirthDate { get; set; }
            [Required]
            [SelectItems(nameof(Countries))]
            [DisplayName("Country")]
            public int CountryId { get; set; }
            [TextArea(Rows = 3)]
            public string Description { get; set; }
            [HiddenInput]
            public bool IsReturnPost { get; set; } = false;
            public int FormIsOk { get; set; }
            public string Sector { get; set; } // iş unvanı
            public string JobPosition { get; set; } // iş unvanı

            public string JobTitle { get; set; } // iş unvanı
            public string IsBecomeConsultDesc { get; set; } // Neden Danışman Olmak İstiyorsunuz?
            public string Profession { get; set; } // Uzmanlık Alanı
            public string IsNeedConsult { get; set; } // İhtiyaç Duyulan Danışman Alanı
            public string EducationStatus { get; set; } // Eğitim Durumu
            public string CompanyName { get; set; } // Şirket Adı
            public string CompanyContact { get; set; } // Şirket İletişim Bilgileri
            public string PositionName { get; set; } // Pozisyon Adı
            public string SpecialRequest { get; set; } // Özel İstekler
            public string JobTip { get; set; } // İş Tanımı
            public string RequiredQualification { get; set; } // Aranan Nitelik
            public string SalaryRange { get; set; } // Maaş Aralığı
            public string HoursWork { get; set; } // Çalışma Saatleri
            public string Location { get; set; } // Lokasyon

            public Guid? UserId { get; set; }

        }
    }
}