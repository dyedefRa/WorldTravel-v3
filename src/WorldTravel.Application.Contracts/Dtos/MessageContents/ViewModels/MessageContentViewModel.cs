using WorldTravel.Enums;
using System;

namespace WorldTravel.Dtos.MessageContents.ViewModels
{
    public class MessageContentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsSeen { get; set; }
        public bool IsMine { get; set; }
        public string MessageSendDate { get; set; }
        //public bool IsTodayDivider { get; set; }
        public Status Status { get; set; }

    }
}
