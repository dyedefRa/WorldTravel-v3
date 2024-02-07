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
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Dtos.VisaTypes.ViewModels;
using WorldTravel.Entities.VisaTypes;
using WorldTravel.Enums;
using WorldTravel.Localization;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Models.Results.Concrete;
using WorldTravel.Permissions;

namespace WorldTravel.Services
{
    [Authorize(WorldTravelPermissions.VisaType.Default)]

    public class VisaTypeAppService : CrudAppService<VisaType, VisaTypeDto, int, PagedAndSortedResultRequestDto, CreateUpdateVisaTypeDto, CreateUpdateVisaTypeDto>, IVisaTypeAppService
    {
        private readonly IStringLocalizer<WorldTravelResource> _L;

        public VisaTypeAppService(
           IRepository<VisaType, int> repository,
           IStringLocalizer<WorldTravelResource> L
           ) : base(repository)
        {
            _L = L;
        }

        public async Task<PagedResultDto<VisaTypeViewModel>> GetVisaTypeListAsync(GetVisaTypeRequestDto input)
        {
            var result = new PagedResultDto<VisaTypeViewModel>();

            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Country)
                .Include(x => x.Image);

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.VisaTitleFilter), x => x.Title.Contains(input.VisaTitleFilter));

            var totalCount = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var viewModel = ObjectMapper.Map<VisaType, VisaTypeViewModel>(x);
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
        /// Web tarafındaki VisaType/Index sayfasında kullanılır.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<VisaTypeViewModel>> GetVisaTypeListForUserAsync(GetVisaTypeRequestDto input)
        {
            var result = new List<VisaTypeViewModel>();
            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Country)
                .Include(x => x.Image);

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.VisaTitleFilter), x => x.Title.Contains(input.VisaTitleFilter))
                .WhereIf(input.IsSeenHomePage == true, x => x.IsSeenHomePage == true);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var viewModel = ObjectMapper.Map<VisaType, VisaTypeViewModel>(x);
                viewModel.CountryName = x.Country.Title;
                viewModel.PreviewImageUrl = x.Image.Path;
                return viewModel;
            }).OrderBy(x => x.Rank).ToList();

            return viewModels;
        }

        [AllowAnonymous]
        public async Task<IDataResult<VisaTypeViewModel>> GetVisaTypeAsync(int visaTypeId)
        {
            try
            {
                var data = await Repository
                    .Include(x => x.Country)
                    .Include(x => x.Image)
                    .Where(x => x.Status == Status.Active)
                    .FirstOrDefaultAsync(x => x.Id == visaTypeId);

                if (data == null)
                    return new ErrorDataResult<VisaTypeViewModel>(L["EntityNotFoundError"]);

                var result = ObjectMapper.Map<VisaType, VisaTypeViewModel>(data);
                result.CountryName = data.Country.Title;
                result.PreviewImageUrl = data.Image.Path;
                result.CountryId = data.CountryId;

                return new SuccessDataResult<VisaTypeViewModel>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "VisaTypeAppService > GetVisaTypeAsync");

                return new ErrorDataResult<VisaTypeViewModel>(L["GeneralError"]);
            }
        }

        #endregion
    }
}
