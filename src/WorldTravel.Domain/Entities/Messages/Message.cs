using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using WorldTravel.Entities.MessageContents;
using WorldTravel.Entities.Users;
using WorldTravel.Enums;

namespace WorldTravel.Entities.Messages
{
    public class Message : Entity<int>
    {
        //Toplam gonderılen mesaj sayısı
        //Toplam konusan kişi sayısı
        // toplam Konuşkan puanı
        //toplam hızlı cevap veren puanı
        //toplam Güvenilir
        //toplam vs
        //yüzdeleri , bugun yuzde x oldu felan
        public MessageStatus SenderStatus { get; set; }
        public MessageStatus ReceiverStatus { get; set; }
        public DateTime SenderStatusDate { get; set; }
        public DateTime ReceiverStatusDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid SenderId { get; set; }
        public virtual AppUser Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public virtual AppUser Receiver { get; set; }

        //ARSIV MI SENDER ARSIV MI REIVECER
        public MessageType MessageType { get; set; }
        public Status Status { get; set; }

        //Puan olayları buraya 6 lı enum olusturher enum ıcın deger ac

        public virtual ICollection<MessageContent> MessageContents { get; set; }
    }
}
