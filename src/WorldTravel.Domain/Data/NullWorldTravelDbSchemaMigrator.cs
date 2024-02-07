using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace WorldTravel.Data
{
    /* This is used if database provider does't define
     * IWorldTravelDbSchemaMigrator implementation.
     */
    public class NullWorldTravelDbSchemaMigrator : IWorldTravelDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}