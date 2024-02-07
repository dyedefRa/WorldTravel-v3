using System;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Jobs
{
    public class CreateUpdateJobDto
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ExtraDescription { get; set; }
        public int ImageId { get; set; }
        public int CountryId { get; set; }
        public string HourlyRate { get; set; }
        public string AccommodationFee { get; set; }
        public string LanguageName { get; set; }
        public int ReadCount { get; set; } = 0;
        public int Rank { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ValidDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Active;
    }
}
