using WorldTravel.Dtos.Users;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace WorldTravel.Abstract
{
    public interface IOverridedIdentityUserAppService : IApplicationService
    {
        Task<PagedResultDto<CustomIdentityUserDto>> GetUserListAsync(GetIdentityUsersInput input);
    }
}