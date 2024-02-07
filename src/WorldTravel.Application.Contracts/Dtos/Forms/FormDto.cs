using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using WorldTravel.Dtos.Countries;
using WorldTravel.Dtos.Users;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Forms
{
    public class FormDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int CountryId { get; set; }
        public CountryDto Country { get; set; }
        public string FormRegType { get; set; }

        #region New Property
        public string Sector { get; set; } // Sektör
        public string JobTitle { get; set; } // iş unvanı
        public string IsBecomeConsultDesc { get; set; } // Neden Danışman Olmak İstiyorsunuz?
        public string Profession { get; set; } // Uzmanlık Alanı
        public string IsNeedConsult { get; set; } // İhtiyaç Duyulan Danışman Alanı
        public string EducationStatus { get; set; } // Eğitim Durumu
        public string CompanyName { get; set; } // Şirket Adı
        public string CompanyContact { get; set; } // Şirket İletişim Bilgileri
        public string PositionName { get; set; } // Pozisyon Adı
        public string JobPosition { get; set; } // Pozisyon Adı
        public string SpecialRequest { get; set; } // Özel İstekler
        public string JobTip { get; set; } // İş Tanımı
        public string RequiredQualification { get; set; } // Aranan Nitelik
        public string SalaryRange { get; set; } // Maaş Aralığı
        public string HoursWork { get; set; } // Çalışma Saatleri
        public string Location { get; set; } // Lokasyon
        #endregion

        public bool IsContacted { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; }
        public Guid UserId { get; set; }
        public int FormIsOk { get; set; }

    }
}
