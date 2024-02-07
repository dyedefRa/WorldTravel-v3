using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Forms;
using WorldTravel.Dtos.VisaTypes.ViewModels;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.VisaType
{
    [AutoValidateAntiforgeryToken]
    public class DetailModel : WorldTravelPageModel
    {
        [BindProperty]
        public VisaTypeViewModel VisaType { get; set; }
        [BindProperty]
        public FormModel Form { get; set; }
        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Countries { get; set; }

        private readonly IVisaTypeAppService _visaTypeAppService;
        private readonly ILookupAppService _lookupAppService;
        private readonly IFormAppService _formAppService;

        public DetailModel(
            IVisaTypeAppService visaTypeAppService,
            ILookupAppService lookupAppService,
            IFormAppService formAppService)
        {
            _visaTypeAppService = visaTypeAppService;
            _lookupAppService = lookupAppService;
            _formAppService = formAppService;
        }

        public async Task<IActionResult> OnGet(int id, bool r = false)
        {
            var data = await _visaTypeAppService.GetVisaTypeAsync(id);
            if (!data.Success)
                return Redirect("~/Error?httpStatusCode=404");
            TempData["surec"] = false;

            VisaType = data.Data;
            Form = new FormModel();
            Form.VisaTypeId = id;
            Genders = _lookupAppService.GetGenderLookup();
            Countries = await _lookupAppService.GetCountryLookupAsync();
            if (r)
            {
                Form.IsReturnPost = true;
            }
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

                        Form.JobTip = "yok";
                        Form.JobPosition = "yok";
                        Form.PositionName = "yok";
                        Form.Sector = "yok";
                        Form.JobTitle = "yok";
                        Form.CompanyContact = "yok";
                        Form.CompanyName = "yok";
                        Form.EducationStatus = "yok";
                        Form.RequiredQualification = "yok";
                        Form.SpecialRequest = "yok";
                        Form.IsNeedConsult = "yok";
                        Form.IsBecomeConsultDesc = "yok";
                        Form.HoursWork = "yok";
                        Form.SalaryRange = "yok";
                        Form.Profession = "yok";
                        Form.Location = "yok";

                        var form = new FormDto
                        {
                            FormRegType = "Vize Başvurusu",
                            SalaryRange = Form.SalaryRange,
                            BirthDate = DateTime.Now,
                            Sector = Form.Sector,
                            SpecialRequest = Form.SpecialRequest,
                            CompanyContact = Form.CompanyContact,
                            CompanyName = Form.CompanyName,
                            RequiredQualification = Form.RequiredQualification,
                            CountryId = Form.CountryId,
                            CreatedDate = DateTime.Now,
                            Description = Form.Description != null || Form.Description != "" ? Form.Description : "yok",
                            EducationStatus = Form.EducationStatus,
                            Email = Form.Email,
                            FormIsOk = 0,
                            Gender = Form.Gender,
                            HoursWork = Form.HoursWork,
                            IsBecomeConsultDesc = Form.IsBecomeConsultDesc,
                            IsContacted = false,
                            IsNeedConsult = Form.IsNeedConsult,
                            JobPosition = Form.JobPosition,
                            JobTip = Form.JobTip,
                            JobTitle = Form.JobTitle,
                            Location = Form.Location,
                            Name = Form.Name,
                            PhoneNumber = Form.PhoneNumber,
                            PositionName = Form.PositionName,
                            Profession = Form.Profession,
                            Surname = Form.Surname,
                            UserId = (Guid)CurrentUser.Id,
                            Status = Status.Active,
                        };

                        //var input = ObjectMapper.Map<FormModel, CreateUpdateFormDto>(Form);
                        var addedResult = await _formAppService.CreateManualAsync(form);

                        if (addedResult != null)
                        {
                            var redirectUrl = $"~/VisaType/Detail?id={Form.VisaTypeId}&r={true}";
                            return Redirect(redirectUrl);
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
                Log.Error(ex, "VisaType > DetailModel > OnPostAsync has error! ");
            }

            Alerts.Danger(L["GeneralError"].Value);
            return Page();
        }

        public class FormModel
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public string Surname { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }


            [Required]
            [Phone]
            [StringLength(16, MinimumLength = 16, ErrorMessage = "Telefon alanı geçerli bir telefon numarası olmalıdır.")]
            public string PhoneNumber { get; set; }
            [Required]
            [SelectItems(nameof(Gender))]
            public GenderType Gender { get; set; }
            public Nullable<DateTime> BirthDate { get; set; }
            [Required]
            [SelectItems(nameof(Countries))]
            public int CountryId { get; set; }
            [TextArea(Rows = 3)]
            public string Description { get; set; }
            [HiddenInput]
            public bool IsReturnPost { get; set; } = false;
            [HiddenInput]
            public int VisaTypeId { get; set; }
            public string Sector { get; set; } // iş unvanı
            public string JobPosition { get; set; }
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
        }
    }
}
