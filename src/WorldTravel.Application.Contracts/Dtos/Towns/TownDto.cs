using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.Towns
{
    public class TownDto : EntityDto<int>
    {
        [MaxLength(255)]
        public string Title { get; set; }
        public int TownNo { get; set; }
        //public int CityId { get; set; }
        //public  CityDto City { get; set; }
        //HERE?
    }
}
