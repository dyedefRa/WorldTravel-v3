using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Pages.Identity;
using WorldTravel.Abstract;
using WorldTravel.Managers;

namespace WorldTravel.Web.Pages.Account
{
    [AutoValidateAntiforgeryToken]
    public class ForgotPasswordModel : IdentityPageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IMailTemplateAppService _mailTemplateAppService;

        public ForgotPasswordModel(
            IConfiguration configuration,
            IMailTemplateAppService mailTemplateAppService
            )
        {
            _configuration = configuration;
            _mailTemplateAppService = mailTemplateAppService;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public IdentityUserManager UserManager { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                var userResult = await UserManager.FindByEmailAsync(Email);
                if (userResult == null)
                {
                    Alerts.Danger(L["UserNotFound"].Value);
                    return Page();
                }
                else
                {
                    var selfUrl = _configuration["App:SelfUrl"];
                    var plainingText = string.Format("email={0}&timestamp={1}", userResult.Email, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    var encryptedText = EncryptionManager.Encrypt(plainingText);
                    var activationUrl = string.Format("{0}/Account/NewPasswordActivation?key={1}", selfUrl, HttpUtility.UrlEncode(encryptedText));

                    var isSend = await _mailTemplateAppService.SendNewPasswordMailAsync(activationUrl, userResult.Email);
                    if (isSend)
                    {
                        Alerts.Success(L["CheckYourMail"].Value);
                        return Page();
                    }
                    else
                    {
                        Alerts.Danger(L["GeneralError"].Value);
                        return Page();
                    }
                }
            }
            catch (Exception)
            {
                Alerts.Danger(L["GeneralError"].Value);
                return Page();
            }
        }
    }
}