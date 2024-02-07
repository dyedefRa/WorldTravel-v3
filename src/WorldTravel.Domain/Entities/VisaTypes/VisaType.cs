using System;
using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.Countries;
using WorldTravel.Entities.Files;
using WorldTravel.Enums;

namespace WorldTravel.Entities.VisaTypes
{
    public class VisaType : Entity<int>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ExtraDescription { get; set; }
        public int ImageId { get; set; }
        public virtual File Image { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int ReadCount { get; set; }
        public int Rank { get; set; }
        public int VisaFee { get; set; }
        public int VisaDuration { get; set; }

        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidDate { get; set; }
        public Status Status { get; set; }


        //YORUM MUHABBETINI SONRA YAPCAZ
    }
}
