using System.Threading.Tasks;

namespace WorldTravel.HangfireServices.Abstract
{
    public interface IRecurringJobService
    {
        Task<bool> SendProductionMailsJob();
    }
}
