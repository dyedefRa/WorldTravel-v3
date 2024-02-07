using WorldTravel.Enums;
using Volo.Abp.Identity;

namespace WorldTravel.Dtos.Users
{
    public class CustomIdentityUserDto : IdentityUserDto
    {
        public UserType? UserType { get; set; }
        public string ImageUrl { get; set; }
        public int? ImageId { get; set; }
        public int? ProfileIsOk { get; set; }
        public Status? Status { get; set; }

    }
}
