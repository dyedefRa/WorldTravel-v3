using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.Cities;

namespace WorldTravel.Entities.Towns
{
    public class Town : Entity<int>
    {
        public string Title { get; set; }
        public int TownNo { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
