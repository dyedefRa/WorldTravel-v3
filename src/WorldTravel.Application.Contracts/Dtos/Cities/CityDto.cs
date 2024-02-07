using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.Cities
{
    public class CityDto : EntityDto<int>
    {
        public string Title { get; set; }
        public int CityNo { get; set; }
        public string AreaNumber { get; set; }
    }
}
