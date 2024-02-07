using WorldTravel.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace WorldTravel.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class WorldTravelPageModel : AbpPageModel
    {
        protected WorldTravelPageModel()
        {
            LocalizationResourceType = typeof(WorldTravelResource);
        }
    }
}