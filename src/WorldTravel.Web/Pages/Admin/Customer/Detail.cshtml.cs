using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity.Web.Pages.Identity;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Forms;
using WorldTravel.Dtos.Forms.ViewModels;
using WorldTravel.Dtos.Receipts;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin.Customer
{
    public class Detail : PageModel
    {
        [Parameter]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public GenderType? Gender { get; set; }
        public DateTime? BirthDate { get; set; }

        private readonly IUserAppService _userAppService;
        public Detail(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task OnGet(Guid id)
        {
            ViewData["id"] = id;
            var userId = id;
            var user = await _userAppService.GetAppUserAsync(userId);
            if (user == null)
            {
                throw new UserFriendlyException("Kullanıcı bulunamadı.");
            }
            else
            {
                Id = id;
                Name = user.Data.Name + " " + user.Data.Surname;
                UserName = user.Data.UserName;
                Email = user.Data.Email;
                BirthDate = user.Data.BirthDate;
                Gender = user.Data.Gender;
            }

        }
    }
}
