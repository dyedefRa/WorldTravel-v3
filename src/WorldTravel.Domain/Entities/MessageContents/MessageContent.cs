using System;
using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.Messages;
using WorldTravel.Enums;

namespace WorldTravel.Entities.MessageContents
{
    public class MessageContent : Entity<int>
    {
        public int MessageId { get; set; }
        public virtual Message Message { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDeletedForSender { get; set; }
        public bool IsDeletedForReceiver { get; set; }
        public DateTime? DeletedDateForSender { get; set; }
        public DateTime? DeletedDateForReceived { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; }

    }
}
