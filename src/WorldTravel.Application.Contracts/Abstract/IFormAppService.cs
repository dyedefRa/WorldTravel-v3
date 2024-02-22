using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WorldTravel.Dtos.Forms;
using WorldTravel.Dtos.Forms.ViewModels;
using WorldTravel.Models.Results.Abstract;

namespace WorldTravel.Abstract
{
    public interface IFormAppService : ICrudAppService<FormDto, int, PagedAndSortedResultRequestDto, CreateUpdateFormDto, CreateUpdateFormDto>
    {
        public Task<PagedResultDto<FormViewModel>> GetFormListAsyncByUserId(GetFormRequestDto input);
        public Task<IDataResult<FormViewModel>> GetFormAsync(int Id);
        public Task<List<FormViewModel>> GetFormListAsyncUserId(Guid userId);
        Task<bool> CreateManualAsync(FormDto form);
        int GetFormCountByUserId(Guid userId);

    }
}
