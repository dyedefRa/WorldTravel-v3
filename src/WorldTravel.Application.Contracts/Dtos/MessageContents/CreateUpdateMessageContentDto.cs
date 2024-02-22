using WorldTravel.Dtos.Messages;
using WorldTravel.Enums;
using System;

namespace WorldTravel.Dtos.MessageContents
{
    public class CreateUpdateMessageContentDto
    {
        public int MessageId { get; set; }
        public MessageDto Message { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public bool IsSeen { get; set; } = false;
        public bool IsDeletedForSender { get; set; } = false;
        public bool IsDeletedForReceiver { get; set; } = false;
        public DateTime? DeletedDateForSender { get; set; }
        public DateTime? DeletedDateForReceived { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Active;
    }
}
