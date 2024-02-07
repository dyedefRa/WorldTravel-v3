using System.Threading.Tasks;

namespace WorldTravel.Data
{
    public interface IWorldTravelDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
