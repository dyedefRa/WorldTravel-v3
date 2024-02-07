using WorldTravel.Dtos.Users.ViewModels;
using WorldTravel.Models.Pages.Account;
using WorldTravel.Models.Results.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using System.Collections.Generic;

namespace WorldTravel.Abstract
{
    public interface IUserAppService : IApplicationService
    {
        Task<IDataResult<AppUserViewModel>> GetAppUserAsync(Guid userId);
        Task<IDataResult<string>> ChangeProfileImageAsync(Guid userId, IFormFile image);
        Task<IDataResult<string>> ManageProfileAsync(Guid userId, ManageProfileModel input);

        //Task<CustomIdentityUserDto> GetUserByIdAsync(Guid userId);
        //Task<AppUserDto> CreateCustomAsync(CreateUpdateAppUserDto input);
    }
}
