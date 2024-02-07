using System;

namespace WorldTravel.Models.Pages.Account
{
    public class ManageProfileModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
