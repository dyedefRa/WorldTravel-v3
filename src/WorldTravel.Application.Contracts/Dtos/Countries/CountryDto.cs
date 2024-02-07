using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.Countries
{
    public class CountryDto : EntityDto<int>
    {
        public string Title { get; set; }

    }
}
