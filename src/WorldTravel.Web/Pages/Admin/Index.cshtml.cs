using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WorldTravel.Entities.Users;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin
{
    [Authorize]
    public class IndexModel : WorldTravelPageModel
    {
        private readonly IRepository<AppUser> _appUserRepository;

        public IndexModel(
            IRepository<AppUser> appUserRepository
            )
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (CurrentUser.IsAuthenticated)
                {
                    var mustBeAdminUser = await _appUserRepository.FirstOrDefaultAsync(x => x.Id == CurrentUser.Id);
                    if ((mustBeAdminUser.UserType != null && mustBeAdminUser.UserType == UserType.User))
                    {
                        return Redirect("~/Home");
                    }
                }
                else
                {
                    return Redirect("~/Account/Login");
                }
            }
            catch (Exception)
            {
                return Redirect("~/Home");
            }

            return Page();
        }

    }
}