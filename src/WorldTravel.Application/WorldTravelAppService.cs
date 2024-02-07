using System;
using System.Collections.Generic;
using System.Text;
using WorldTravel.Localization;
using Volo.Abp.Application.Services;

namespace WorldTravel
{
    /* Inherit your application services from this class.
     */
    public abstract class WorldTravelAppService : ApplicationService
    {
        protected WorldTravelAppService()
        {
            LocalizationResource = typeof(WorldTravelResource);
        }
    }
}
