using WorldTravel.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace WorldTravel.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class WorldTravelController : AbpController
    {
        protected WorldTravelController()
        {
            LocalizationResource = typeof(WorldTravelResource);
        }
    }
}