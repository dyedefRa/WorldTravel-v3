using System;
using System.Collections.Generic;
using System.Text;
using WorldTravel.Dtos.Files;
using WorldTravel.Dtos.Users;

namespace WorldTravel.Models.Pages.Account
{
    public class ReceiptModel
    {
        public int FileId { get; set; }
        public Guid UserId { get; set; }
        public bool ReceiptOk { get; set; }
    }
}
