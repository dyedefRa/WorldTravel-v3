using Volo.Abp.Application.Dtos;
using WorldTravel.Dtos.CountryContents;
using WorldTravel.Dtos.Files;

namespace WorldTravel.Entities.CountryContentFiles
{
    public class CountryContentFileDto : EntityDto<int>
    {
        public int CountryContentId { get; set; }
        public CountryContentDto CountryContent { get; set; }
        public int FileId { get; set; }
        public FileDto File { get; set; }
        public bool IsShareContent { get; set; }
    }
}
