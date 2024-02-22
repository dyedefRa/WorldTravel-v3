using System;
using Volo.Abp.Application.Dtos;
using WorldTravel.Dtos.Messages;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.MessageContents
{
    public class MessageContentDto : EntityDto<int>
    {
        public int MessageId { get; set; }
        public MessageDto Message { get; set; }
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
