using System;

namespace WorldTravel.Dtos.Receipts
{
    public class CreateUpdateReceiptDto
    {
        public int FileId { get; set; }
        //public FileDto File { get; set; }
        public Guid UserId { get; set; }
        //public AppUserDto User { get; set; }

        public bool ReceiptOk { get; set; }
    }
}
