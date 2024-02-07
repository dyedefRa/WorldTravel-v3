using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WorldTravel.Dtos.Forms;
using System.Collections.Generic;
using WorldTravel.Dtos.Forms.ViewModels;
using WorldTravel.Models.Results.Abstract;
using System.Linq;
using WorldTravel.Models.Results.Concrete;
using WorldTravel.Dtos.Receipts;

namespace WorldTravel.Abstract
{
    public interface IFormAppService : ICrudAppService<FormDto, int, PagedAndSortedResultRequestDto, CreateUpdateFormDto, CreateUpdateFormDto>
    {
        public Task<PagedResultDto<FormViewModel>> GetFormListAsyncByUserId(GetFormRequestDto input);
        public Task<IDataResult<FormViewModel>> GetFormAsync(int Id);
        public Task<List<FormViewModel>> GetFormListAsyncUserId(Guid userId);
        Task<bool> CreateManualAsync(FormDto form);

    }


}
