using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Receipts;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin.Customer
{
    public class FormDetail : PageModel
    {

        //admin 1q2w3E*

        [Parameter]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int FormIsOk { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType? Gender { get; set; }

        #region New Property
        public string Sector { get; set; } // iþ unvaný
        public string JobPosition { get; set; } // Pozisyon Adý
        public string JobTitle { get; set; } // iþ unvaný
        public string IsBecomeConsultDesc { get; set; } // Neden Danýþman Olmak Ýstiyorsunuz?
        public string Profession { get; set; } // Uzmanlýk Alaný
        public string IsNeedConsult { get; set; } // Ýhtiyaç Duyulan Danýþman Alaný
        public string EducationStatus { get; set; } // Eðitim Durumu
        public string CompanyName { get; set; } // Þirket Adý
        public string CompanyContact { get; set; } // Þirket Ýletiþim Bilgileri
        public string PositionName { get; set; } // Pozisyon Adý
        public string SpecialRequest { get; set; } // Özel Ýstekler
        public string JobTip { get; set; } // Ýþ Tanýmý
        public string RequiredQualification { get; set; } // Aranan Nitelik
        public string SalaryRange { get; set; } // Maaþ Aralýðý
        public string HoursWork { get; set; } // Çalýþma Saatleri
        public string Location { get; set; } // Lokasyon

        #endregion

        private readonly IFormAppService _formAppService;

        public FormDetail(IFormAppService formAppService)
        {
            _formAppService = formAppService;
        }

        public async Task OnGet(int id)
        {
            Id = id;
            var form = await _formAppService.GetFormAsync(id);
            if (form == null)
            {
                throw new UserFriendlyException("Form bulunamadý.");
            }
            else
            {
                FullName = form.Data.Name;
                Email = form.Data.Email;
                PhoneNumber = form.Data.PhoneNumber;
                Gender = form.Data.Gender;
                FormIsOk = form.Data.FormIsOk;

                Sector = form.Data.Sector;
                JobPosition = form.Data.JobPosition;
                JobTitle = form.Data.JobTitle;
                IsBecomeConsultDesc = form.Data.IsBecomeConsultDesc;
                Profession = form.Data.Profession;

                IsNeedConsult = form.Data.IsNeedConsult;
                EducationStatus = form.Data.EducationStatus;
                CompanyName = form.Data.CompanyName;
                CompanyContact = form.Data.CompanyContact;
                PositionName = form.Data.PositionName;

                SpecialRequest = form.Data.SpecialRequest;
                JobTip = form.Data.JobTip;
                RequiredQualification = form.Data.RequiredQualification;
                SalaryRange = form.Data.SalaryRange;
                HoursWork = form.Data.HoursWork;
                Location = form.Data.Location;

            }
        }
    }
}
