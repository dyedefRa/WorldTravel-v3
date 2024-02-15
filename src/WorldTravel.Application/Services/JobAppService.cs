using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Dtos.Jobs.ViewModels;
using WorldTravel.Entities.Jobs;
using WorldTravel.Enums;
using WorldTravel.Localization;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Models.Results.Concrete;
using WorldTravel.Permissions;

namespace WorldTravel.Services
{
    [Authorize(WorldTravelPermissions.Job.Default)]
    public class JobAppService : CrudAppService<Job, JobDto, int, PagedAndSortedResultRequestDto, CreateUpdateJobDto, CreateUpdateJobDto>, IJobAppService
    {
        private readonly IStringLocalizer<WorldTravelResource> _L;

        public JobAppService(
           IRepository<Job, int> repository,
           IStringLocalizer<WorldTravelResource> L
           ) : base(repository)
        {
            _L = L;
        }

        public async Task<PagedResultDto<JobViewModel>> GetJobListAsync(GetJobRequestDto input)
        {
            var result = new PagedResultDto<JobViewModel>();

            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Country)
                .Include(x => x.Image);

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), x => x.Title.Contains(input.JobTitleFilter));

            var totalCount = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var viewModel = ObjectMapper.Map<Job, JobViewModel>(x);
                viewModel.CountryName = x.Country.Title;
                viewModel.PreviewImageUrl = x.Image.Path;
                return viewModel;
            }).ToList();

            result.Items = viewModels;
            result.TotalCount = totalCount;
            return result;
        }

        public async Task SoftDeleteAsync(int Id)
        {
            var entity = Repository.FirstOrDefault(x => x.Id == Id);
            entity.Status = Enums.Status.Deleted;
            await Repository.UpdateAsync(entity);
        }

        #region User Pages

        /// <summary>
        /// Web tarafındaki Job/Index sayfasında kullanılır.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<JobViewModel>> GetJobListForUserAsync(GetJobRequestDto input)
        {
            input.MaxResultCount = 50;
            var result = new List<JobViewModel>();
            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Country)
                .Include(x => x.Image);

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), x => x.Title.Contains(input.JobTitleFilter))
                .WhereIf(input.IsSeenHomePage == true, x => x.IsSeenHomePage == true);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var viewModel = ObjectMapper.Map<Job, JobViewModel>(x);
                viewModel.CountryName = x.Country.Title;
                viewModel.PreviewImageUrl = x.Image.Path;
                return viewModel;
            }).OrderBy(x => x.Rank).ToList();

            return viewModels;
        }

        [AllowAnonymous]
        public async Task<IDataResult<JobViewModel>> GetJobAsync(int jobId)
        {
            try
            {
                var data = await Repository
                    .Include(x => x.Country)
                    .Include(x => x.Image)
                    .Where(x => x.Status == Status.Active)
                    .FirstOrDefaultAsync(x => x.Id == jobId);

                if (data == null)
                    return new ErrorDataResult<JobViewModel>(L["EntityNotFoundError"]);

                var result = ObjectMapper.Map<Job, JobViewModel>(data);
                result.CountryName = data.Country.Title;
                result.PreviewImageUrl = data.Image.Path;
                result.CountryId = data.CountryId;

                return new SuccessDataResult<JobViewModel>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "VisaTypeAppService > GetVisaTypeAsync");

                return new ErrorDataResult<JobViewModel>(L["GeneralError"]);
            }
        }

        #endregion
    }
}
