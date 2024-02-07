using System;
using Volo.Abp.Application.Dtos;
using WorldTravel.Dtos.Countries;
using WorldTravel.Dtos.Files;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Blog
{
    public class BlogDto : EntityDto<int>
    {
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string Description { get; set; }
        public string SeoDescription { get; set; }
        public int ImageId { get; set; }
        public FileDto Image { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidDate { get; set; }
        public Status Status { get; set; }
    }
}
