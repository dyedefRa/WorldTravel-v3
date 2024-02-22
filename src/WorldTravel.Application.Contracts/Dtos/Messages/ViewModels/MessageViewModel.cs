using WorldTravel.Dtos.MessageContents.ViewModels;
using WorldTravel.Enums;
using System;
using System.Collections.Generic;

namespace WorldTravel.Dtos.Messages.ViewModels
{
    public class MessageViewModel
    {
        //public MessageViewModel()
        //{
        //    MessageContents = new List<MessageContentDto>();
        //}
        public int Id { get; set; }
        public MessageStatus SenderStatus { get; set; }
        public MessageStatus ReceiverStatus { get; set; }
        //public DateTime SenderStatusDate { get; set; }
        //public DateTime ReceiverStatusDate { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public Guid SenderId { get; set; }
        //public AppUserDto Sender { get; set; }
        //public Guid ReceiverId { get; set; }
        //public AppUserDto Receiver { get; set; }
        //public int CreatedById { get; set; }
        //public int FirstReceivedById { get; set; }
        public MessageType MessageType { get; set; }
        //public Status Status { get; set; }
        //public AppUserDto ShownUser { get; set; }
        public string ShownUserImageUrl { get; set; }
        public string ShownUserFullName { get; set; }
        public string ShownUserZodiacSign { get; set; }
        public string ShownUserLastSeenDate { get; set; }
        public int UnSeenMessageCount { get; set; }
        public DateTime LastMessageDate { get; set; }
        public bool IsBlocked { get; set; } = false;
        public bool IsMuted { get; set; } = false;
        //public bool IsMainUser { get; set; }
        //public List<MessageContentDto> MessageContents { get; set; }
    }


    public class MessageWithContentViewModel
    {
        public MessageWithContentViewModel()
        {
            MessageContents = new List<MessageContentViewModel>();
        }
        public int MessageId { get; set; }
        public Guid ShownUserId { get; set; }
        public string ShownUserFullName { get; set; }
        public string ShownUserImageUrl { get; set; }
        //public bool LoadMoreMessage { get; set; }
        public bool IsBlocked { get; set; } = false;
        public bool IsMuted { get; set; } = false;

        public List<MessageContentViewModel> MessageContents { get; set; }
    }
}
