using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.VisaTypes
{
    public class GetVisaTypeRequestDto : PagedAndSortedResultRequestDto
    {
        public string VisaTitleFilter { get; set; }
        public bool IsSeenHomePage { get; set; }
    }
}
