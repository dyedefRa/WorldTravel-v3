using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WorldTravel.Abstract
{
    public interface ILookupAppService : IApplicationService
    {
        Task<List<SelectListItem>> GetCityLookupAsync();
        Task<List<SelectListItem>> GetTownLookupAsync(int cityId);
        Task<List<SelectListItem>> GetCountryLookupAsync();
        List<SelectListItem> GetGenderLookup();
    }
}
