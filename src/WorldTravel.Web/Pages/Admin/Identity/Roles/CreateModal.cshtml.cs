using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Pages.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace WorldTravel.Web.Pages.Admin.Identity.Roles
{
    public class CreateModalModel : IdentityPageModel
    {
        [BindProperty]
        public RoleInfoModel Role { get; set; }

        protected IIdentityRoleAppService IdentityRoleAppService { get; }

        public CreateModalModel(IIdentityRoleAppService identityRoleAppService)
        {
            IdentityRoleAppService = identityRoleAppService;
        }

        public virtual Task<IActionResult> OnGetAsync()
        {
            Role = new RoleInfoModel();

            return Task.FromResult<IActionResult>(Page());
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<RoleInfoModel, IdentityRoleCreateDto>(Role);
            await IdentityRoleAppService.CreateAsync(input);

            return NoContent();
        }

        public class RoleInfoModel : ExtensibleObject
        {
            [Required]
            [DynamicStringLength(typeof(IdentityRoleConsts), nameof(IdentityRoleConsts.MaxNameLength))]
            [Display(Name = "RoleName")]
            public string Name { get; set; }

            [Display(Name = "DisplayName:IsDefault")]
            public bool IsDefault { get; set; } = false;

            [Display(Name = "DisplayName:IsPublic")]
            public bool IsPublic { get; set; } = false;
        }
    }
}