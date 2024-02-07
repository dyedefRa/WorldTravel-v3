using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.Blog
{
    public class GetBlogRequestDto : PagedAndSortedResultRequestDto
    {
        public string BlogTitleFilter { get; set; }
        public bool IsSeenHomePage { get; set; }
    }
}
