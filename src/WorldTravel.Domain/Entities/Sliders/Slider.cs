using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.Files;
using WorldTravel.Enums;

namespace WorldTravel.Entities.Sliders
{
    public class Slider : Entity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Rank { get; set; }      
        public int ImageId { get; set; }
        public virtual File Image { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidDate { get; set; }
        public Status Status { get; set; }
    }
}
