using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Pages.Identity;
using WorldTravel.Abstract;
using WorldTravel.Enums;
using WorldTravel.Extensions;

namespace WorldTravel.Web.Pages.Account
{
    [AutoValidateAntiforgeryToken]
    public class RegisterModel : IdentityPageModel
    {
        [BindProperty]
        public UserRegisterModel UserRegisterInputModel { get; set; }
        public List<SelectListItem> Genders { get; set; }

        private readonly ILookupAppService _lookupAppService;
        protected IIdentityUserAppService _identityUserAppService { get; }
        private IdentityUserManager _identityUserManager;
        private SignInManager<Volo.Abp.Identity.IdentityUser> _signInManager;

        public RegisterModel(
            ILookupAppService lookupAppService,
            IIdentityUserAppService identityUserAppService,
            IdentityUserManager identityUserManager,
            SignInManager<Volo.Abp.Identity.IdentityUser> signInManager
            )
        {
            _lookupAppService = lookupAppService;
            _identityUserAppService = identityUserAppService;
            _identityUserManager = identityUserManager;
            _signInManager = signInManager;
        }


        public void OnGet()
        {
            UserRegisterInputModel = new UserRegisterModel();
            Genders = _lookupAppService.GetGenderLookup();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var input = ObjectMapper.Map<UserRegisterModel, IdentityUserCreateDto>(UserRegisterInputModel);

                    input.ExtraProperties.Add("UserType", (int)UserType.User);
                    input.ExtraProperties.Add("Gender", (int)UserRegisterInputModel.Gender);
                    input.ExtraProperties.Add("BirthDate", UserRegisterInputModel.BirthDate);
                    input.ExtraProperties.Add("ProfileIsOk", 0);
                    input.ExtraProperties.Add("Status", (int)Status.Active);
                    input.UserName = StringExtensions.GenerateUserName(UserRegisterInputModel.Email);
                    input.RoleNames = new string[] { EnumExtensions.GetEnumDescription(UserType.User) };
                    
                    var addedResult = await _identityUserAppService.CreateAsync(input);

                    //(await UserManager.CreateAsync(user)).CheckErrors(LocalizationManager);
                    if (addedResult != null)
                    {
                        var user = await _identityUserManager.GetByIdAsync(addedResult.Id);

                        await _signInManager.SignInAsync(user, true);

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    Alerts.Danger(L["GeneralIdentityError"].Value);
                }
            }
            catch (AbpIdentityResultException ex)
            {
                var identityError = ex.IdentityResult;

                if (!identityError.Succeeded && identityError.Errors.Any())
                {
                    var totalErrorCount = identityError.Errors.Count();
                    if (identityError.Errors.Any(x => x.Code == "DuplicateUserName"))
                    {
                        Alerts.Danger($"'{UserRegisterInputModel.UserName}' kullanıcı adına sahip bir kullanıcı zaten var.");
                        totalErrorCount--;
                    }

                    if (identityError.Errors.Any(x => x.Code == "DuplicateEmail"))
                    {
                        Alerts.Danger($"'{UserRegisterInputModel.Email}' mailine sahip bir kullanıcı zaten var.");
                        totalErrorCount--;
                    }

                    if (totalErrorCount > 0)
                    {
                        Alerts.Danger(L["GeneralIdentityError"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CompanyRegisterModel > OnPostAsync has error! ");
                Alerts.Danger(L["GeneralIdentityError"].Value);
            }

            return Page();
        }

        public class UserRegisterModel
        {

            //[Required]
            //[RegularExpression(@"^[0-9a-zA-Z]{4,20}$", ErrorMessage = "Kullanıcı Adı alanı en az 4, en fazla 20 karakterden olmalıdır.İçerisinde sadece harf yada sayı bulunmalıdır.")]
            public string UserName { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Surname { get; set; }
            [Required]
            [DataType(DataType.Password)]
            //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Parola en az 6 karakterden oluşmalı.İçinde büyük harf, küçük harf, sayı ve simge bulunmalıdır.")]
            //[DisableAuditing]
            [MinLength(6, ErrorMessage = "Parola en az 6 karakter olmalıdır.")]
            public string Password { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "Parolalar uyuşmuyor.")]
            public string ConfirmPassword { get; set; }
            [Required]
            [EmailAddress]
            //[RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
            public string Email { get; set; }
            [Required]
            [StringLength(16, MinimumLength = 16, ErrorMessage = "Telefon alanı geçerli bir telefon numarası olmalıdır.")]
            public string PhoneNumber { get; set; }
            [Required]
            [SelectItems(nameof(Gender))]
            public GenderType Gender { get; set; }
            public int ProfileIsOk { get; set; }
            public DateTime? BirthDate { get; set; }
        }
    }
}
