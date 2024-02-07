using WorldTravel.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace WorldTravel.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(WorldTravelEntityFrameworkCoreModule),
        typeof(WorldTravelApplicationContractsModule)
        )]
    public class WorldTravelDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
