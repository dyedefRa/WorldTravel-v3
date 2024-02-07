using System;

namespace WorldTravel.Dtos.VisaTypes.ViewModels
{
    public class VisaTypeViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ExtraDescription { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string PreviewImageUrl { get; set; }
        public int ReadCount { get; set; }
        public int Rank { get; set; }
        public int VisaFee { get; set; }
        public int VisaDuration { get; set; }

        public bool IsSeenHomePage { get; set; }
        public int TotalImageCount { get; set; }
        public int TotalVideoCount { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
