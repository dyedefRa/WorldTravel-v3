using WorldTravel.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace WorldTravel
{
    [DependsOn(
        typeof(WorldTravelEntityFrameworkCoreTestModule)
        )]
    public class WorldTravelDomainTestModule : AbpModule
    {

    }
}