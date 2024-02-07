using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Dtos.Jobs.ViewModels;
using WorldTravel.Models.Results.Abstract;

namespace WorldTravel.Abstract
{
    public interface IJobAppService : ICrudAppService<JobDto, int, PagedAndSortedResultRequestDto, CreateUpdateJobDto, CreateUpdateJobDto>
    {
        Task SoftDeleteAsync(int Id);
        Task<List<JobViewModel>> GetJobListForUserAsync(GetJobRequestDto input);
        Task<IDataResult<JobViewModel>> GetJobAsync(int jobId);
    }
}
