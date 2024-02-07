using System;
using System.Collections.Generic;
using System.Text;
using WorldTravel.Dtos.Files;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Sliders
{
    public class CreateUpdateSliderDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int ImageId { get; set; }
        public int Rank { get; set; }
        public virtual FileDto Image { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ValidDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Active;
    }
}
