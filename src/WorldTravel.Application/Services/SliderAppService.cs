using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Sliders;
using WorldTravel.Dtos.Sliders.ViewModels;
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Dtos.VisaTypes.ViewModels;
using WorldTravel.Entities.Sliders;
using WorldTravel.Enums;
using WorldTravel.Localization;
using WorldTravel.Permissions;

namespace WorldTravel.Services
{
    [Authorize(WorldTravelPermissions.Slider.Default)]
    public class SliderAppService : CrudAppService<Slider, SliderDto, int, PagedAndSortedResultRequestDto, CreateUpdateSliderDto, CreateUpdateSliderDto>, ISliderAppService
    {
        private readonly IStringLocalizer<WorldTravelResource> _L;

        public SliderAppService(
           IRepository<Slider, int> repository,
           IStringLocalizer<WorldTravelResource> L
           ) : base(repository)
        {
            _L = L;
        }

        [AllowAnonymous]
        public async Task<PagedResultDto<SliderViewModel>> GetSliderListAsync(GetSliderRequestDto input)
        {
            var result = new PagedResultDto<SliderViewModel>();

            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Image);

            query = query
           .WhereIf(!string.IsNullOrWhiteSpace(input.SliderTitleFilter), x => x.Title.Contains(input.SliderTitleFilter));

            var totalCount = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var viewModel = ObjectMapper.Map<Slider, SliderViewModel>(x);
                viewModel.Title = x.Title;
                viewModel.IsSeenHomePage = x.IsSeenHomePage;
                viewModel.PreviewImageUrl= x.Image.Path;
                viewModel.Url = x.Url;
                viewModel.Rank = x.Rank;
                viewModel.CreatedDate = x.CreatedDate;
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

        [AllowAnonymous]
        public async Task<List<SliderViewModel>> GetSliderListHomeAsync(GetSliderRequestDto input)
        {
            var result = new PagedResultDto<SliderViewModel>();

            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Image);

            query = query
           .WhereIf(!string.IsNullOrWhiteSpace(input.SliderTitleFilter), x => x.Title.Contains(input.SliderTitleFilter));

            var totalCount = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var viewModel = ObjectMapper.Map<Slider, SliderViewModel>(x);
                viewModel.Title = x.Title;
                viewModel.IsSeenHomePage = x.IsSeenHomePage;
                viewModel.PreviewImageUrl = x.Image.Path;
                viewModel.Url = x.Url;
                viewModel.Rank = x.Rank;
                viewModel.CreatedDate = x.CreatedDate;
                return viewModel;
            }).ToList();

            return viewModels;
        }
    }
}
