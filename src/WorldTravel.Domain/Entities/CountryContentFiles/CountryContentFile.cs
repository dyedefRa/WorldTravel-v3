using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.CountryContents;
using WorldTravel.Entities.Files;

namespace WorldTravel.Entities.CountryContentFiles
{
    public class CountryContentFile : Entity<int>
    {
        public int CountryContentId { get; set; }
        public virtual CountryContent CountryContent { get; set; }
        public int FileId { get; set; }
        public virtual File File { get; set; }
        public bool IsShareContent { get; set; }
    }
}
