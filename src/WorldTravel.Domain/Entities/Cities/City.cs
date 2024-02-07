using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.Towns;

namespace WorldTravel.Entities.Cities
{
    public class City : Entity<int>
    {
        public string Title { get; set; }
        public int CityNo { get; set; }
        public string AreaNumber { get; set; }
        public virtual ICollection<Town> Towns { get; set; }
    }
}
