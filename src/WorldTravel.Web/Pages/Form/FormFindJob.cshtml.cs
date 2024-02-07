using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Forms;
using WorldTravel.Enums;
using Serilog;
using Microsoft.AspNetCore.Components;

namespace WorldTravel.Web.Pages.Form
{
    [AutoValidateAntiforgeryToken]
    public class FormFindJobModel : WorldTravelPageModel
    {

        [Parameter]
        public string FormRegType { get; set; }

        [BindProperty]
        public FormModel Form { get; set; }

        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Countries { get; set; }

        private readonly ILookupAppService _lookupAppService;
        private readonly IFormAppService _formAppService;

        public FormFindJobModel(
               ILookupAppService lookupAppService,
               IFormAppService formAppService
               )
        {
            _lookupAppService = lookupAppService;
            _formAppService = formAppService;
        }
        private async Task LoadInitializeData()
        {
            Form = new FormModel();
            Genders = _lookupAppService.GetGenderLookup();
            Countries = await _lookupAppService.GetCountryLookupAsync();
            ViewData["RegType"] = FormRegType;
        }

        public async Task<IActionResult> OnGet(string formRegisterType = "")
        {
            if(CurrentUser.Id != null)
            {
                FormRegType = formRegisterType;
                await LoadInitializeData();
                return Page();
            }

            else
            {
                return RedirectToAction("Login", "Account");
            }
          
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //return Redirect("~/Error?httpStatusCode=404");
            try
            {
                if (ModelState.IsValid)
                {
                    if(Form.FormRegType == "vizebasvurusu")
                    {
                        Form.FormRegType = "Vize Ba�vurusu";
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
                        Form.CountryId = 190;
                        Form.Profession = "yok";
                        Form.Location = "yok";
                    }
                    else if(Form.FormRegType == "iscibul")
                    {
                        Form.FormRegType = "���i Bul";
                        Form.Description = "";
                        Form.Gender = GenderType.DoNotWantSpecify;
                        Form.BirthDate = DateTime.Now;
                        Form.CountryId = 190;
                        Form.JobPosition = "yok";
                        Form.PositionName = "yok";
                        Form.Sector = "yok";
                        Form.Profession = "yok";
                        Form.JobTitle = "yok";
                        Form.EducationStatus = "yok";
                        Form.RequiredQualification = "yok";
                        Form.SpecialRequest = "yok";
                        Form.IsNeedConsult = "yok";
                        Form.IsBecomeConsultDesc = "yok";
                    }
                    else if (Form.FormRegType == "danismanbul")
                    {
                        Form.FormRegType = "Dan��man Bul";
                        Form.Description = "yok";
                        Form.Gender = GenderType.DoNotWantSpecify;
                        Form.BirthDate = DateTime.Now;
                        Form.JobTip = "yok";
                        Form.PositionName = "yok";
                        Form.Sector = "yok";
                        Form.JobTitle = "yok";
                        Form.EducationStatus = "yok";
                        Form.JobPosition = "yok";
                        Form.CompanyContact = "yok";
                        Form.CompanyName = "yok";
                        Form.RequiredQualification = "yok";
                        Form.SpecialRequest = "yok";
                        Form.IsBecomeConsultDesc = "yok";
                        Form.HoursWork = "yok";
                        Form.SalaryRange = "yok";
                        Form.Profession = "yok";    
                        Form.Location = "yok";
                        Form.CountryId = 190;

                    }
                    else if (Form.FormRegType == "danismanol")
                    {
                        Form.FormRegType = "Dan��man Ol";
                        Form.Description = "yok";
                        Form.Gender = GenderType.DoNotWantSpecify;
                        Form.BirthDate = DateTime.Now;
                        Form.CountryId = 190;
                        Form.JobTip = "yok";
                        Form.JobPosition = "yok";
                        Form.PositionName = "yok";
                        Form.CompanyContact = "yok";
                        Form.CompanyName = "yok";   
                        Form.EducationStatus = "yok";
                        Form.IsBecomeConsultDesc = "yok";
                        Form.RequiredQualification = "yok";
                        Form.SpecialRequest = "yok";
                        Form.IsNeedConsult = "yok";
                        Form.HoursWork = "yok";
                        Form.SalaryRange = "yok";
                        Form.Location = "yok";
                        Form.Profession = "yok";
                    }
                    else if (Form.FormRegType == "isbul")
                    {
                        Form.FormRegType = "�� Bul";
                        Form.Description = "yok";
                        Form.Gender = GenderType.DoNotWantSpecify;
                        Form.BirthDate = DateTime.Now;
                        Form.JobTip = "yok";
                        Form.PositionName = "yok";
                        Form.Sector = "yok";
                        Form.JobTitle = "yok";
                        Form.CompanyContact = "yok";
                        Form.CompanyName = "yok";
                        Form.RequiredQualification = "yok";
                        Form.SpecialRequest = "yok";
                        Form.IsNeedConsult = "yok";
                        Form.IsBecomeConsultDesc = "yok";
                        Form.HoursWork = "yok";
                        Form.SalaryRange = "yok";
                        Form.Profession = "yok";
                        Form.Location = "yok";
                    }

                    else
                    {

                    }

                    var form = new FormDto
                    {
                        FormRegType = Form.FormRegType,
                        SalaryRange= Form.SalaryRange,
                        BirthDate= DateTime.Now,
                        Sector= Form.Sector,
                        SpecialRequest = Form.SpecialRequest,
                        CompanyContact= Form.CompanyContact,
                        CompanyName= Form.CompanyName,  
                        RequiredQualification= Form.RequiredQualification,
                        CountryId= Form.CountryId,
                        CreatedDate = DateTime.Now,
                        Description = Form.Description != null || Form.Description != "" ? Form.Description : "yok",
                        EducationStatus = Form.EducationStatus,
                        Email= Form.Email,
                        FormIsOk = 0,
                        Gender= Form.Gender,
                        HoursWork= Form.HoursWork,
                        IsBecomeConsultDesc= Form.IsBecomeConsultDesc,
                        IsContacted = false,
                        IsNeedConsult= Form.IsNeedConsult,
                        JobPosition= Form.JobPosition,
                        JobTip= Form.JobTip,
                        JobTitle= Form.JobTitle,
                        Location= Form.Location,
                        Name= Form.Name,
                        PhoneNumber= Form.PhoneNumber,
                        PositionName= Form.PositionName,
                        Profession = Form.Profession,
                        Surname= Form.Surname,
                        UserId = (Guid)CurrentUser.Id,
                        Status = Status.Active,
                    };

                    //var input = ObjectMapper.Map<FormModel, CreateUpdateFormDto>(Form);
                    var addedResult = await _formAppService.CreateManualAsync(form);
                    if (addedResult != null)
                    {
                        return RedirectToAction("Manage", "Account", new { returnPost = true });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Form > IndexModel > OnPostAsync has error! ");
            }

            Alerts.Danger(L["GeneralError"].Value);
            return Page();
        }

        public class FormModel
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }

            [SelectItems(nameof(Gender))]
            public GenderType Gender { get; set; }  
            public Nullable<DateTime> BirthDate { get; set; }
            [SelectItems(nameof(Countries))]
            public int CountryId { get; set; }

            #region New Property

            public string Sector { get; set; } // Sekt�r

            public string JobTitle { get; set; } // i� unvan�

            public string IsBecomeConsultDesc { get; set; } // Neden Dan��man Olmak �stiyorsunuz?

            public string Profession { get; set; } // Uzmanl�k Alan�

            public string IsNeedConsult { get; set; } // �htiya� Duyulan Dan��man Alan�

            public string EducationStatus { get; set; } // E�itim Durumu
            public string CompanyName { get; set; } // �irket Ad�
            public string CompanyContact { get; set; } // �irket �leti�im Bilgileri
            public string PositionName { get; set; } // Pozisyon Ad�
            public string JobPosition { get; set; } // Pozisyon Ad�
            public string SpecialRequest { get; set; } // �zel �stekler
            public string JobTip { get; set; } // �� Tan�m�
            public string RequiredQualification { get; set; } // Aranan Nitelik
            public string SalaryRange { get; set; } // Maa� Aral���
            public string HoursWork { get; set; } // �al��ma Saatleri
            public string Location { get; set; } // Lokasyon
            #endregion

            public string FormRegType { get; set; }

            [TextArea(Rows = 3)]
            public string Description { get; set; }
        }
    }
}
