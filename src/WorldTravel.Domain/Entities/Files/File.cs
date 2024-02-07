using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.Blogs;
using WorldTravel.Entities.CountryContentFiles;
using WorldTravel.Entities.CountryContents;
using WorldTravel.Entities.Jobs;
using WorldTravel.Entities.Receipts;
using WorldTravel.Entities.Sliders;
using WorldTravel.Entities.Users;
using WorldTravel.Entities.VisaTypes;
using WorldTravel.Enums;

namespace WorldTravel.Entities.Files
{
    public class File : Entity<int>
    {
        public string Name { get; set; }
        public long? Size { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public FileType FileType { get; set; }
        public int Rank { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; }

        public virtual AppUser User { get; set; }
        public virtual ICollection<CountryContent> CountryContents { get; set; }
        public virtual ICollection<CountryContentFile> CountryContentFiles { get; set; }
        public virtual ICollection<Slider> Slider { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<VisaType> VisaTypes { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Receipt> ReceiptList { get; set; }
    }
}
