using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Users;
using WorldTravel.Entities.Users;

namespace WorldTravel.Services
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IIdentityUserAppService), typeof(IdentityUserAppService), typeof(OverridedIdentityUserAppService))]
    public class OverridedIdentityUserAppService : IdentityUserAppService, IOverridedIdentityUserAppService
    {

        private readonly IRepository<AppUser> _appUserRepository;
        public OverridedIdentityUserAppService(IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IOptions<IdentityOptions> identityOptions,
            IRepository<AppUser> appUserRepository) : base(userManager, userRepository, roleRepository, identityOptions)
        {

            _appUserRepository = appUserRepository;
        }

        //Buraya düşüyor mu kontrol et
        public async Task<PagedResultDto<CustomIdentityUserDto>> GetUserListAsync(GetIdentityUsersInput input)
        {
            var count = await UserRepository.GetCountAsync(input.Filter);
            var list = (await UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter)).Select(x =>
            {
                var dto = ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, CustomIdentityUserDto>(x);
                return dto;
            }).ToList();

            var appUsers = await _appUserRepository/*.Include(x => x.Image)*/
                .Where(x => list.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            foreach (var item in list)
            {
                var user = appUsers?.FirstOrDefault(x => x.Id == item.Id);
                //item.ImageUrl = user?.Image?.FilePath;
                //item.Status = user?.Status;
                //item.UserType = user?.UserType;
            }

            return new PagedResultDto<CustomIdentityUserDto>(count, list);
        }

        /// <summary>
        /// /Account/Register kullanıyor.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async override Task<IdentityUserDto> GetAsync(Guid id)
        {
            /*var addedUser =*/
            return await base.GetAsync(id);
            //if (addedUser!=null)
            //{
            //    if (input.RoleNames.Any(x=>x=="gamer"))
            //    {
            //        var userGameList = await _gameRepository.Where(x => x.Status == Enums.Status.Active).Select(x => new UserGame
            //        {
            //            GameId = x.Id,
            //            GameUniqueId = string.Empty,
            //            TypeId = 0,
            //            UserId = addedUser.Id

            //        }).ToListAsync();
            //        await _userGameRepository.InsertManyAsync(userGameList);
            //    }


            //}
            //return addedUser;
        }

        /// <summary>
        /// /Account/Register kullanıyor.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async override Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            /*var addedUser =*/
            return await base.CreateAsync(input);
            //if (addedUser!=null)
            //{
            //    if (input.RoleNames.Any(x=>x=="gamer"))
            //    {
            //        var userGameList = await _gameRepository.Where(x => x.Status == Enums.Status.Active).Select(x => new UserGame
            //        {
            //            GameId = x.Id,
            //            GameUniqueId = string.Empty,
            //            TypeId = 0,
            //            UserId = addedUser.Id

            //        }).ToListAsync();
            //        await _userGameRepository.InsertManyAsync(userGameList);
            //    }


            //}
            //return addedUser;
        }

        /// <summary>
        /// /Account/Register kullanıyor.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async override Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            /*var addedUser =*/
            return await base.UpdateAsync(id, input);
            //if (addedUser!=null)
            //{
            //    if (input.RoleNames.Any(x=>x=="gamer"))
            //    {
            //        var userGameList = await _gameRepository.Where(x => x.Status == Enums.Status.Active).Select(x => new UserGame
            //        {
            //            GameId = x.Id,
            //            GameUniqueId = string.Empty,
            //            TypeId = 0,
            //            UserId = addedUser.Id

            //        }).ToListAsync();
            //        await _userGameRepository.InsertManyAsync(userGameList);
            //    }


            //}
            //return addedUser;
        }
    }
}
