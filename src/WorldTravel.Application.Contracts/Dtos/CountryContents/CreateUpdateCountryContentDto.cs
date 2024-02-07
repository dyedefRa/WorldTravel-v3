using System;
using System.Collections.Generic;
using WorldTravel.Entities.CountryContentFiles;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.CountryContents
{
    public class CreateUpdateCountryContentDto
    {
        public CreateUpdateCountryContentDto()
        {
            CountryContentFiles = new List<CountryContentFileDto>();
        }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ExtraDescription { get; set; }
        public int ImageId { get; set; }
        public int CountryId { get; set; }
        public int ReadCount { get; set; } = 0;
        public int Rank { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ValidDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Active;
        public List<CountryContentFileDto> CountryContentFiles { get; set; }
    }
}
