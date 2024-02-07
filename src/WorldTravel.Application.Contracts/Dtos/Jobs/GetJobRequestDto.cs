using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.Jobs
{
    public class GetJobRequestDto : PagedAndSortedResultRequestDto
    {
        public string JobTitleFilter { get; set; }
        public bool IsSeenHomePage { get; set; }
    }
}
