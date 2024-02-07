using System;
using Volo.Abp.Domain.Entities;
using WorldTravel.Enums;

namespace WorldTravel.Entities.MailTemplates
{
    public class MailTemplate : Entity<int>
    {
        public string Subject { get; set; }
        public string MailKey { get; set; }
        public string MailTemplateValue { get; set; }
        public DateTime InsertedDate { get; set; }
        public Status Status { get; set; }
    }
}
