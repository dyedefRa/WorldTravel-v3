using WorldTravel.Enums;
using System;
using System.Collections.Generic;
using WorldTravel.Dtos.Forms;

namespace WorldTravel.Dtos.Users.ViewModels
{
    public class AppUserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public UserType? UserType { get; set; }
        public GenderType? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ProfileIsOk { get; set; }
        public int? ImageId { get; set; }
        public string ImageUrl { get; set; }
        public Status? Status { get; set; }
        public int FormCount { get; set; } = 0;
        public virtual ICollection<FormDto> Forms { get; set; }

    }
}
