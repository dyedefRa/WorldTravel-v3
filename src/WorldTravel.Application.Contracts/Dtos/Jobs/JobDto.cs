using System;
using Volo.Abp.Application.Dtos;
using WorldTravel.Dtos.Countries;
using WorldTravel.Dtos.Files;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Jobs
{
    public class JobDto : EntityDto<int>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ExtraDescription { get; set; }
        public string HourlyRate { get; set; }
        public string AccommodationFee { get; set; }
        public string LanguageName { get; set; }
        public int ImageId { get; set; }
        public FileDto Image { get; set; }
        public int CountryId { get; set; }
        public CountryDto Country { get; set; }
        public int ReadCount { get; set; }
        public int Rank { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidDate { get; set; }
        public Status Status { get; set; }
    }
}
