using WorldTravel.Dtos.MessageContents;
using WorldTravel.Dtos.Users;
using WorldTravel.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.Messages
{
    public class MessageDto : EntityDto<int>
    {
        public MessageDto()
        {
            MessageContents = new List<MessageContentDto>();
        }

        public MessageStatus SenderStatus { get; set; }
        public MessageStatus ReceiverStatus { get; set; }
        public DateTime SenderStatusDate { get; set; }
        public DateTime ReceiverStatusDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid SenderId { get; set; }
        public virtual AppUserDto Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public virtual AppUserDto Receiver { get; set; }
        public MessageType MessageType { get; set; }
        public Status Status { get; set; }

        public List<MessageContentDto> MessageContents { get; set; }
    }
}
