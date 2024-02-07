using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WorldTravel.Dtos.CountryContents;
using WorldTravel.Dtos.CountryContents.ViewModels;
using WorldTravel.Models.Results.Abstract;

namespace WorldTravel.Abstract
{
    public interface ICountryContentAppService : ICrudAppService<CountryContentDto, int, PagedAndSortedResultRequestDto, CreateUpdateCountryContentDto, CreateUpdateCountryContentDto>
    {
        Task<IDataResult<bool>> AddOrRemoveCountryContentFileRelationAsync(int countryContentId, List<int> newFileIds, List<int> removedFileIds = null, bool isShareContent = false);

        Task SoftDeleteAsync(int Id);

        Task<List<CountryContentViewModel>> GetCountryContentListForUserAsync(GetCountryContentRequestDto input);

        Task<IDataResult<CountryContentViewModel>> GetCountryContentAsync(int countryContentId);
    }
}
