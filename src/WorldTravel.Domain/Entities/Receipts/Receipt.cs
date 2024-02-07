using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.Files;
using WorldTravel.Entities.Forms;
using WorldTravel.Entities.Users;

namespace WorldTravel.Entities.Receipts
{
    public class Receipt : Entity<int>
    {
        public int FileId { get; set; }
        public File File { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public bool ReceiptOk { get; set; }
    }
}
