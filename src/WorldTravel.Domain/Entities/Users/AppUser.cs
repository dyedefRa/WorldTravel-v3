using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Users;
using WorldTravel.Entities.Files;
using WorldTravel.Entities.Forms;
using WorldTravel.Entities.Receipts;
using WorldTravel.Enums;

namespace WorldTravel.Entities.Users
{
    public class AppUser : FullAuditedAggregateRoot<Guid>, IUser
    {
        #region Base properties 

        public virtual Guid? TenantId { get; private set; }
        public virtual string UserName { get; private set; }
        public virtual string Name { get; private set; }
        public virtual string Surname { get; private set; }
        public virtual string Email { get; private set; }
        public virtual bool EmailConfirmed { get; private set; }
        public virtual string PhoneNumber { get; private set; }
        public virtual bool PhoneNumberConfirmed { get; private set; }

        #endregion

        public UserType? UserType { get; set; }
        public GenderType? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ProfileIsOk { get; set; }
        public virtual int? ImageId { get; set; }
        public virtual File Image { get; set; }
        public Status? Status { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<Receipt> ReceiptList { get; set; }

    }
}
