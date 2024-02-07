using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Dtos.Sliders;
using WorldTravel.Dtos.Sliders.ViewModels;
using WorldTravel.Dtos.VisaTypes;

namespace WorldTravel.Abstract
{
    public interface ISliderAppService : ICrudAppService<SliderDto, int, PagedAndSortedResultRequestDto, CreateUpdateSliderDto, CreateUpdateSliderDto>
    {
        Task SoftDeleteAsync(int Id);

        Task<PagedResultDto<SliderViewModel>> GetSliderListAsync(GetSliderRequestDto input);
        Task<List<SliderViewModel>> GetSliderListHomeAsync(GetSliderRequestDto input);


    }
}
