using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.Sliders
{
    public class GetSliderRequestDto : PagedAndSortedResultRequestDto
    {
        public string SliderTitleFilter { get; set; }
        public bool IsSeenHomePage { get; set; }
    }
}
