using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.Countries;
using WorldTravel.Entities.Files;
using WorldTravel.Enums;

namespace WorldTravel.Entities.Categories
{
    public class Category : Entity<int>
    {
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string Description { get; set; }
        public string SeoDescription { get; set; }
        public bool IsSeenHomePage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidDate { get; set; }
        public Status Status { get; set; }
    }
}
