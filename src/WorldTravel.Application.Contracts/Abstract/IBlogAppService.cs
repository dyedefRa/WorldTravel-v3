using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WorldTravel.Dtos.Blog;
using WorldTravel.Dtos.VisaTypes.ViewModels;
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Dtos.Blog.ViewModels;
using WorldTravel.Dtos.Jobs;

namespace WorldTravel.Abstract
{
    public interface IBlogAppService : ICrudAppService<BlogDto, int, PagedAndSortedResultRequestDto, CreateUpdateBlogDto, CreateUpdateBlogDto> {
        Task SoftDeleteAsync(int Id);
        Task<List<BlogViewModel>> GetBlogListForAsync(GetBlogRequestDto input);
        Task<PagedResultDto<BlogViewModel>> GetBlogListAsync(GetBlogRequestDto input);

        Task<IDataResult<BlogViewModel>> GetBlogAsync(int postId);
    }
}
