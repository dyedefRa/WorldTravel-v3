using WorldTravel.Abstract;
using WorldTravel.Enums;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;

namespace WorldTravel.Web.Pages.Account
{
    public class LoginModel : AccountPageModel
    {
        [BindProperty]
        public UserLoginModel UserLoginInputModel { get; set; }
        private readonly IAuditingManager _auditingManager;
        private readonly IUserAppService _userAppService;

        public LoginModel(
             IAuditingManager auditingManager,
             IUserAppService userAppService
            )
        {
            _auditingManager = auditingManager;
            _userAppService = userAppService;
        }

        public async Task OnGet()
        {
            UserLoginInputModel = new UserLoginModel();
            await SignInManager.SignOutAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userResult = await UserManager.FindByEmailAsync(UserLoginInputModel.Email);
                    if (userResult == null)
                    {
                        
                        await SignInManager.SignOutAsync();
                        Alerts.Danger("Kullanıcı bulunamadı. Lütfen bilgilerinizi kontrol ediniz.");
                        return Page();
                    }

                    var currentUser = await _userAppService.GetAppUserAsync(userResult.Id);

                    if (currentUser.Data.Status != Status.Active)
                    {
                        await SignInManager.SignOutAsync();
                        Alerts.Warning("Hesabınız aktif değil.");
                        _auditingManager.Current.Log.Comments.Add("Hesabınız aktif değil.{ " + userResult.UserName + " }");
                        return Page();
                    }
                    if (currentUser.Data.UserType != UserType.User)
                    {
                        await SignInManager.SignOutAsync();
                        Alerts.Warning("Kullanıcı hesabınızla giriş yapınız.");
                        _auditingManager.Current.Log.Comments.Add("Kullanıcı hesabınızla giriş yapınız.{ " + userResult.UserName + " }");
                        return Page();
                    }

                    var result = await SignInManager.PasswordSignInAsync(userResult, UserLoginInputModel.Password, true, false);
                    if (result == null)
                    {
                        Alerts.Warning("Kullanıcı bulunamadı.");
                        _auditingManager.Current.Log.Comments.Add("Kullanıcı bulunamadı.{ " + userResult.UserName + " }");
                        return Page();
                    }
                    if (result.IsLockedOut)
                    {
                        Alerts.Warning("Kullanıcı kilitli.");
                        _auditingManager.Current.Log.Comments.Add("Kullanıcı kilitli.{ " + userResult.UserName + " }");
                        return Page();
                    }
                    if (result.IsNotAllowed)
                    {
                        Alerts.Warning("Giriş izniniz yok.");
                        _auditingManager.Current.Log.Comments.Add("Giriş izniniz yok.{ " + userResult.UserName + " }");
                        return Page();
                    }
                    if (!result.Succeeded)
                    {
                        Alerts.Danger("Geçersiz email veya şifre.");
                        _auditingManager.Current.Log.Comments.Add("Geçersiz kullanıcı adı veya şifre.{ " + userResult.UserName + " }");

                        return Page();
                    }
                    Debug.Assert(userResult != null, nameof(userResult) + " != null");
                    _auditingManager.Current.Log.Comments.Add("Giriş yapıldı.{ " + userResult.UserName + " }");

                    return RedirectToAction("Manage", "Account");
                }
                else
                {
                    Alerts.Danger("Geçersiz email veya şifre.");
                    return Page();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Account/Login > OnPostAsync has error!");
                Alerts.Danger("Bir hata oluştu.Lütfen tekrar deneyiniz");
                return Page();
            }
        }

        public class UserLoginModel
        {
            [Required]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string Password { get; set; }
        }
    }
}
