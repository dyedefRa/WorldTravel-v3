using System;
using System.Collections.Generic;
using WorldTravel.Dtos.Files.ViewModels;

namespace WorldTravel.Dtos.CountryContents.ViewModels
{
    public class CountryContentViewModel
    {
        public CountryContentViewModel()
        {
            ImageFiles = new List<FileViewModel>();
            VideoFiles = new List<FileViewModel>();
        }
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
        public bool IsSeenHomePage { get; set; }
        public int TotalImageCount { get; set; }
        public int TotalVideoCount { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<FileViewModel> ImageFiles { get; set; }
        public List<FileViewModel> VideoFiles { get; set; }
    }
}
