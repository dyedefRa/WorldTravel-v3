using System;
using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.SentMails
{
    public class SentMailDto : EntityDto<int>
    {
        public string ToAddress { get; set; }
        public string CcAddress { get; set; }
        public string BccAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}