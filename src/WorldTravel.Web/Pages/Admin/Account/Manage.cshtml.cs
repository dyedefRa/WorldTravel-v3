using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Forms.ViewModels;

namespace WorldTravel.Web.Pages.Admin.Account
{
    public class ManageModel : AccountPageModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        [BindProperty]
        public ChangePasswordViewModel FormModel { get; set; }

        private readonly IIdentityUserAppService _identityUserAppService;

        public ManageModel(IIdentityUserAppService identityUserAppService)
        {
            _identityUserAppService = identityUserAppService;

        }

        public async Task OnGetAsync()
        {
            try
            {
                var userId = CurrentUser.Id;
                var user = await _identityUserAppService.GetAsync(userId ?? Guid.Empty);
                if (user == null)
                {
                    throw new UserFriendlyException("Kullanıcı bulunamadı.");
                }
                else
                {
                    Name = user.Name + " " + user.Surname;
                    UserName = user.UserName;
                    Email = user.Email;

                    //Role = user.ExtraProperties["RoleName"].ToString();
                }
            }
            catch (Exception ex)
            {
                var ss = ex.Message;
                throw;
            }
           
        }
        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();
            var user = await UserManager.FindByIdAsync(CurrentUser.Id.Value.ToString());
            var result = await UserManager.ChangePasswordAsync(user, FormModel.CurrentPassword, FormModel.NewPassword);
            if (!result.Succeeded)
            {
                Alerts.Warning("Şifreniz değiştirilemedi. Lütfen daha sonra tekrar deneyiniz.");
                return Page();
            }
            return Redirect("/");
        }

        public class ChangePasswordViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mevcut Şifre")]
            public string CurrentPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Yeni Şifre")]
            public string NewPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Yeni Şifre Tekrar")]
            [Compare(nameof(NewPassword), ErrorMessage = "Şifreler aynı değil.")]
            public string NewPasswordConfirm { get; set; }
        }
    }
}
