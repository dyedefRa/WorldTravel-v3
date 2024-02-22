using WorldTravel.Dtos.MessageContents;
using WorldTravel.Enums;
using System;
using System.Collections.Generic;

namespace WorldTravel.Dtos.Messages
{
    public class CreateUpdateMessageDto
    {
        public CreateUpdateMessageDto()
        {
            MessageContents = new List<MessageContentDto>();
        }

        public MessageStatus SenderStatus { get; set; } = MessageStatus.Default;
        public MessageStatus ReceiverStatus { get; set; } = MessageStatus.Default;
        public DateTime SenderStatusDate { get; set; } = DateTime.Now;
        public DateTime ReceiverStatusDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public MessageType MessageType { get; set; } = MessageType.User;
        public Status Status { get; set; } = Status.Active;

        public List<MessageContentDto> MessageContents { get; set; }
    }
}
