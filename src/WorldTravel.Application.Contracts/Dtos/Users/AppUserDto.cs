using WorldTravel.Dtos.Files;
using WorldTravel.Enums;
using System;
using Volo.Abp.Application.Dtos;
using System.Collections;
using WorldTravel.Dtos.Forms;
using System.Collections.Generic;

namespace WorldTravel.Dtos.Users
{
    public class AppUserDto : EntityDto<Guid>
    {
        public Guid? TenantId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public UserType? UserType { get; set; }
        public GenderType? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ProfileIsOk { get; set; }
        public int? ImageId { get; set; }
        public FileDto Image { get; set; }
        public Status? Status { get; set; }
        public virtual ICollection<FormDto> FormList { get; set; }

    }
}
