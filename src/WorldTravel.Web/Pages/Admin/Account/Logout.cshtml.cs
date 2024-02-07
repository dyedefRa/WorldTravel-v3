using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Identity.Web.Pages.Identity;

namespace WorldTravel.Web.Pages.Admin.Account
{
    public class LogoutModel : IdentityPageModel
    {
        private SignInManager<Volo.Abp.Identity.IdentityUser> _signInManager;
        public LogoutModel(
                SignInManager<Volo.Abp.Identity.IdentityUser> signInManager
            )
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();

            return Redirect("~/Home");
        }
    }
}
