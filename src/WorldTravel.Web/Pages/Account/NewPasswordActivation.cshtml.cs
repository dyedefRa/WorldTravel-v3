using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Account.Web.Pages.Account;
using WorldTravel.Entities.Users;
using WorldTravel.Managers;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Models.Results.Concrete;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace WorldTravel.Web.Pages.Account
{
    [AutoValidateAntiforgeryToken]
    public class NewPasswordActivation : AccountPageModel
    {

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Boş geçilemez.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Parola en az 6 karakter olmalıdır.")]
        [BindProperty(SupportsGet = true)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Boş geçilemez.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Parola en az 6 karakter olmalıdır.")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
        public bool HasTimeOut { get; set; } = false;
        public bool HasError { get; set; } = false;

        public void OnGet(string key)
        {
            try
            {
                var decryptedText = EncryptionManager.Decrypt(key);
                var keyEmail = decryptedText.Split('&')[0].Split('=')[1];
                var keyTimestamp = DateTime.ParseExact(decryptedText.Split('&')[1].Split('=')[1], "yyyyMMddHHmmss", null);
                Email = keyEmail;
                if (keyTimestamp.AddMinutes(15) < DateTime.Now)
                    HasTimeOut = true;
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(Email);
                var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
                //var resetToken2= resetToken.Replace(" ", "+"); 

                var result = await UserManager.ResetPasswordAsync(user, resetToken, Password);
                //if (!result.Succeeded)
                //{
                //    Alerts.Warning("Şifreniz değiştirilemedi. Lütfen daha sonra tekrar deneyiniz.");
                //    return Page();
                //}

                return RedirectToAction("ChangePasswordResult", "Account", new { result = result.Succeeded });

            }
            catch (Exception)
            {
                HasError = true;
                return RedirectToAction("ChangePasswordResult", "Account", new { result = false });
            }
        }
    }
}