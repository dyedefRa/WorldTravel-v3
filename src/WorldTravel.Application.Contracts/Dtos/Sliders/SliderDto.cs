using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using WorldTravel.Dtos.Files;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Sliders
{
    public class SliderDto : EntityDto<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int ImageId { get; set; }
        public int Rank { get; set; }
        public virtual FileDto Image { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidDate { get; set; }
        public Status Status { get; set; }
    }
}
