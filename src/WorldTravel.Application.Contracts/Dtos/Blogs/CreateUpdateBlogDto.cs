using System;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Blog
{
    public class CreateUpdateBlogDto
    {
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string Description { get; set; }
        public string SeoDescription { get; set; }
        public int ImageId { get; set; }
        public int ReadCount { get; set; } = 0;
        public int Rank { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ValidDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Active;
    }
}
