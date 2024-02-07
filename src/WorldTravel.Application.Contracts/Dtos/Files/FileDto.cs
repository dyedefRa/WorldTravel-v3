using WorldTravel.Enums;
using Volo.Abp.Application.Dtos;
using System;

namespace WorldTravel.Dtos.Files
{
    public class FileDto : EntityDto<int>
    {
        public string Name { get; set; }
        public long? Size { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public FileType FileType { get; set; }
        public int Rank { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; }

    }
}
