using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.CountryContents
{
    public class GetCountryContentRequestDto : PagedAndSortedResultRequestDto
    {
        public string CountryNameFilter { get; set; }
        public bool IsSeenHomePage { get; set; }

    }
}
