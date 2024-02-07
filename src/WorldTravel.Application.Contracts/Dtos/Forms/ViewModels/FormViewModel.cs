using System;
using System.Collections.Generic;
using WorldTravel.Dtos.Countries;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Forms.ViewModels
{
    public class FormViewModel
    {

        public FormViewModel()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public GenderType Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string CountryName { get; set; }
        public CountryDto Country { get; set; }
        public bool IsContacted { get; set; }
        public string FormRegType { get; set; }

        #region New Property
        public string Sector { get; set; } // iş unvanı
        public string JobPosition { get; set; } // Pozisyon Adı
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
        #endregion
        public DateTime CreatedDate { get; set; }
        public int FormIsOk { get; set; }

    }
}
