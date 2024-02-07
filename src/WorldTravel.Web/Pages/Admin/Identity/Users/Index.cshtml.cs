using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Identity.Web.Pages.Identity;

namespace WorldTravel.Web.Pages.Admin.Identity.Users;

public class IndexModel : IdentityPageModel
{
    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
