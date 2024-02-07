using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using WorldTravel.Entities.Users;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin.Account
{
    [AllowAnonymous]
    public class LoginModel : AccountPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }
        [BindProperty]
        public LoginInputModel LoginInput { get; set; }
        public bool EnableLocalLogin { get; set; }
        public IEnumerable<ExternalProviderModel> ExternalProviders { get; set; }
        public IEnumerable<ExternalProviderModel> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));
        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

        protected IAuthenticationSchemeProvider SchemeProvider { get; }
        private readonly IAuditingManager _auditingManager;
        private readonly IRepository<AppUser> _appUserRepository;

        protected AbpAccountOptions AccountOptions { get; }

        public LoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IAuditingManager auditingManager,
            IRepository<AppUser> appUserRepository,
            IOptions<AbpAccountOptions> accountOptions
            )
        {
            SchemeProvider = schemeProvider;
            _auditingManager = auditingManager;
            _appUserRepository = appUserRepository;
            AccountOptions = accountOptions.Value;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginInputModel();
            ExternalProviders = await GetExternalProviders();
            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            if (IsExternalLoginOnly)
            {
                throw new NotImplementedException();
            }
            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync(string action)
        {
            try
            {
                await CheckLocalLoginAsync();
                ValidateModel();
                ExternalProviders = await GetExternalProviders();
                EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);
                await ReplaceEmailToUsernameOfInputIfNeeds();

                var userResult = await UserManager.FindByNameAsync(LoginInput.UserName);
                if (userResult == null)
                {
                    await SignInManager.SignOutAsync();
                    Alerts.Warning("Kullanıcı bulunamadı. Lütfen bilgilerinizi kontrol ediniz.");
                    return Page();
                }

                var mustBeAdminUser = await _appUserRepository.FirstOrDefaultAsync(x => x.Id == userResult.Id);
                //if (mustBeAdminUser.Status != Status.Active)
                //{
                //    await SignInManager.SignOutAsync();
                //    Alerts.Warning("Hesabınız aktif değil.");
                //    return Page();
                //}
                if (mustBeAdminUser.UserType != null && mustBeAdminUser.UserType == UserType.User)
                {
                    await SignInManager.SignOutAsync();
                    Alerts.Warning("Bu sayfadan sadece admin kullanıcıları giriş yapabilir.");
                    return Page();
                }

                var result = await SignInManager.PasswordSignInAsync(LoginInput.UserName, LoginInput.Password, LoginInput.RememberMe, true);
                if (result == null)
                {
                    Alerts.Warning("Kullanıcı bulunamadı.");
                    _auditingManager.Current.Log.Comments.Add("Kullanıcı bulunamadı.{ " + LoginInput.UserName + " }");
                    return Page();
                }
                if (result.IsLockedOut)
                {
                    Alerts.Warning("Kullanıcı kilitli.");
                    _auditingManager.Current.Log.Comments.Add("Kullanıcı kilitli.{ " + LoginInput.UserName + " }");
                    return Page();
                }
                if (result.IsNotAllowed)
                {
                    Alerts.Warning("Giriş izniniz yok.");
                    _auditingManager.Current.Log.Comments.Add("Giriş izniniz yok.{ " + LoginInput.UserName + " }");
                    return Page();
                }
                if (!result.Succeeded)
                {
                    Alerts.Danger("Geçersiz kullanıcı adı veya şifre.");
                    _auditingManager.Current.Log.Comments.Add("Geçersiz kullanıcı adı veya şifre.{ " + LoginInput.UserName + " }");

                    return Page();
                }
                Debug.Assert(userResult != null, nameof(userResult) + " != null");
                _auditingManager.Current.Log.Comments.Add("Giriş yapıldı.{ " + LoginInput.UserName + " }");

                //return RedirectSafely(ReturnUrl, ReturnUrlHash);
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Admin > LoginModel > OnPostAsync has error! ");
                Alerts.Danger("Bir hata oluştu.Lütfen tekrar deneyiniz");
                return Page();
            }
        }

        protected virtual async Task<List<ExternalProviderModel>> GetExternalProviders()
        {
            var schemes = await SchemeProvider.GetAllSchemesAsync();

            return schemes
                .Where(x => x.DisplayName != null || x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                .Select(x => new ExternalProviderModel
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                })
                .ToList();
        }

        protected virtual async Task CheckLocalLoginAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
            }
        }

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds()
        {
            if (!ValidationHelper.IsValidEmailAddress(LoginInput.UserName))
            {
                return;
            }

            var userByUserName = await UserManager.FindByNameAsync(LoginInput.UserName);
            if (userByUserName != null)
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(LoginInput.UserName);
            if (userByEmail == null)
            {
                return;
            }
            LoginInput.UserName = userByEmail.UserName;
        }

        public class LoginInputModel
        {
            [Required]
            [Display(Name = "Kullanıcı Adı")]
            public string UserName { get; set; }
            [Required]
            [Display(Name = "Şifre")]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string Password { get; set; }
            [Display(Name = "Beni Hatırla")]
            public bool RememberMe { get; set; }
        }

        public class ExternalProviderModel
        {
            public string DisplayName { get; set; }
            public string AuthenticationScheme { get; set; }
        }
    }
}