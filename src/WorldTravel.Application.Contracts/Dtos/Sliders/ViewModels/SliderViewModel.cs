using System;
using System.Collections.Generic;
using System.Text;
using WorldTravel.Dtos.Files;

namespace WorldTravel.Dtos.Sliders.ViewModels
{
    public class SliderViewModel
    {
        public SliderViewModel()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Rank { get; set; }
        public string PreviewImageUrl { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
