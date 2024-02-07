using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace WorldTravel.Web
{
    [Dependency(ReplaceServices = true)]
    public class WorldTravelBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "WorldTravel";
    }
}
