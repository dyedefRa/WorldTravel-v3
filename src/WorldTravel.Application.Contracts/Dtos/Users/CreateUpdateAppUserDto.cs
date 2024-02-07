using WorldTravel.Enums;

namespace WorldTravel.Dtos.Users
{
    public class CreateUpdateAppUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserType? UserType { get; set; }
        //public CompanyDto Company { get; set; }
        public int? ImageId { get; set; }
        //public FileDto Image { get; set; }
        public int? ProfileIsOk { get; set; }

        public Status? Status { get; set; }

    }
}
