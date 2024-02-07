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
using WorldTravel.Dtos.Blog;
using WorldTravel.Dtos.Blog.ViewModels;
using WorldTravel.Entities.Blogs;
using WorldTravel.Enums;
using WorldTravel.Localization;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Models.Results.Concrete;
using WorldTravel.Permissions;

namespace WorldTravel.Services
{
    [Authorize(WorldTravelPermissions.Blog.Default)]
    public class BlogAppService  : CrudAppService<Blog, BlogDto, int, PagedAndSortedResultRequestDto, CreateUpdateBlogDto, CreateUpdateBlogDto>, IBlogAppService
    {
        private readonly IStringLocalizer<WorldTravelResource> _L;

        public BlogAppService(IRepository<Blog, int> repository, IStringLocalizer<WorldTravelResource> L) :base(repository)
        {
            _L = L;
        }


        [AllowAnonymous]
        public async Task<IDataResult<BlogViewModel>> GetBlogAsync(int postId)
        {
            try
            {
                var data = await Repository
                    .Include(x => x.Image)
                    .Where(x => x.Status == Status.Active)
                    .FirstOrDefaultAsync(x => x.Id == postId);

                if (data == null)
                    return new ErrorDataResult<BlogViewModel>(L["EntityNotFoundError"]);

                var result = ObjectMapper.Map<Blog, BlogViewModel>(data);
                result.Title = data.Title;
                result.SeoDescription = data.SeoDescription;
                result.SeoTitle = data.SeoTitle;
                result.Description = data.Description;
                result.PreviewImageUrl = data.Image.Path;

                return new SuccessDataResult<BlogViewModel>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BlogAppService > GetBlogAsync");

                return new ErrorDataResult<BlogViewModel>(L["GeneralError"]);
            }
        }

        public async Task<PagedResultDto<BlogViewModel>> GetBlogListAsync(GetBlogRequestDto input)
        {
            var result = new PagedResultDto<BlogViewModel>();

            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Image);

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.BlogTitleFilter), x => x.Title.Contains(input.BlogTitleFilter));

            var totalCount = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var viewModel = ObjectMapper.Map<Blog, BlogViewModel>(x);
                viewModel.Title = x.Title;
                viewModel.SeoDescription = x.SeoDescription;
                viewModel.SeoTitle = x.SeoTitle;
                viewModel.Description = x.Description;
                viewModel.PreviewImageUrl = x.Image.Path;
                viewModel.IsSeenHomePage = x.IsSeenHomePage;
                viewModel.CreatedDate = x.CreatedDate;
                return viewModel;
            }).ToList();

            result.Items = viewModels;
            result.TotalCount = totalCount;
            return result;
        }

        [AllowAnonymous]
        public async Task<List<BlogViewModel>> GetBlogListForAsync(GetBlogRequestDto input)
        {
            var result = new PagedResultDto<BlogViewModel>();

            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Image);

            query = query
            .WhereIf(!string.IsNullOrWhiteSpace(input.BlogTitleFilter), x => x.Title.Contains(input.BlogTitleFilter))
            .WhereIf(input.IsSeenHomePage == true, x => x.IsSeenHomePage == true);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var totalCount = await query.CountAsync();

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var viewModel = ObjectMapper.Map<Blog, BlogViewModel>(x);
                viewModel.Title = x.Title;
                viewModel.SeoDescription = x.SeoDescription;
                viewModel.SeoTitle = x.SeoTitle;
                viewModel.Description = x.Description;
                viewModel.PreviewImageUrl = x.Image.Path;
                viewModel.IsSeenHomePage= x.IsSeenHomePage;
                viewModel.CreatedDate = x.CreatedDate;
                return viewModel;
            }).ToList();

            return viewModels;
        }

        public async Task SoftDeleteAsync(int Id)
        {
            var entity = Repository.FirstOrDefault(x => x.Id == Id);
            entity.Status = Enums.Status.Deleted;
            await Repository.UpdateAsync(entity);
        }
    }
}
