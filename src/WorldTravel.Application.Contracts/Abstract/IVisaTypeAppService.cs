using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Dtos.VisaTypes.ViewModels;
using WorldTravel.Models.Results.Abstract;

namespace WorldTravel.Abstract
{
    public interface IVisaTypeAppService : ICrudAppService<VisaTypeDto, int, PagedAndSortedResultRequestDto, CreateUpdateVisaTypeDto, CreateUpdateVisaTypeDto>
    {
        Task SoftDeleteAsync(int Id);
        Task<List<VisaTypeViewModel>> GetVisaTypeListForUserAsync(GetVisaTypeRequestDto input);
        Task<IDataResult<VisaTypeViewModel>> GetVisaTypeAsync(int visaTypeId);
    }
}
