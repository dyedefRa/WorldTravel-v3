using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using WorldTravel.Dtos.Files;
using WorldTravel.Dtos.Users;

namespace WorldTravel.Dtos.Receipts
{
    public class ReceiptDto : EntityDto<int>
    {
        public int FileId { get; set; }
        public FileDto File { get; set; }
        public Guid UserId { get; set; }
        public AppUserDto User { get; set; }

        public bool ReceiptOk { get; set; }
    }
}
