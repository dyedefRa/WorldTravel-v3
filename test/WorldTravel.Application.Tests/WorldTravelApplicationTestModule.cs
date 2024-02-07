using Volo.Abp.Modularity;

namespace WorldTravel
{
    [DependsOn(
        typeof(WorldTravelApplicationModule),
        typeof(WorldTravelDomainTestModule)
        )]
    public class WorldTravelApplicationTestModule : AbpModule
    {

    }
}