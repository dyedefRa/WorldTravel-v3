using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorldTravel.Abstract;
using WorldTravel.Entities.Cities;
using WorldTravel.Entities.Countries;
using WorldTravel.Entities.Towns;
using WorldTravel.Enums;
using WorldTravel.Extensions;

namespace WorldTravel.Services
{
    [AllowAnonymous]
    public class LookupAppService : ApplicationService, ILookupAppService
    {
        private readonly IRepository<City, int> _cityRepository;
        private readonly IRepository<Town, int> _townRepository;
        private readonly IRepository<Country, int> _countryRepository;

        public LookupAppService(
            IRepository<City, int> cityRepository,
            IRepository<Town, int> townRepository,
            IRepository<Country, int> countryRepository
            )
        {
            _cityRepository = cityRepository;
            _townRepository = townRepository;
            _countryRepository = countryRepository;
        }

        public async Task<List<SelectListItem>> GetCityLookupAsync()
        {
            try
            {
                var list = await _cityRepository.ToListAsync();
                return list.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LookupAppService > CityLookupAsync has error! ");
                return null;
            }
        }

        public async Task<List<SelectListItem>> GetTownLookupAsync(int cityId)
        {
            try
            {
                var list = await _townRepository.Where(x => x.CityId == cityId).OrderBy(x => x.Title).ToListAsync();

                return list.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LookupAppService > GetTownLookupAsync has error! ");
                return null;
            }
        }

        public async Task<List<SelectListItem>> GetCountryLookupAsync()
        {
            try
            {
                var list = await _countryRepository.ToListAsync();
                return list.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LookupAppService > GetCountryLookupAsync has error! ");
                return null;
            }
        }

        public List<SelectListItem> GetGenderLookup()
        {
            try
            {
                var list = ((GenderType[])Enum.GetValues(typeof(GenderType))).ToList();
                return list.Select(x => new SelectListItem(x.ToDescription(), x.ToString())).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LookupAppService > GetGenderLookup has error! ");
                return null;
            }
        }

        //public async Task<List<SelectListItem>> GetOrganizationLookupAsync()
        //{
        //    try
        //    {
        //        var list = await _organizationRepository.Where(x => x.Status == Status.Active).ToListAsync();

        //        return list.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "LookupAppService > GetOrganizationLookupAsync has error! ");
        //        return null;
        //    }
        //}
        ////KULLANILMIYOR
        //public async Task<List<SelectListItem>> GetUserByRoleAsync(UserRoleType userRoleType)
        //{
        //    try
        //    {
        //        var list = await _userManager.GetUsersInRoleAsync(userRoleType.ToDescription());

        //        return list.Select(x => new SelectListItem(x.Name + " " + x.Surname, x.Id.ToString())).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "UserAppService > GetUserByRoleAsync ");
        //        return null;
        //    }
        //}


        //public async Task<List<SelectListItem>> GetLookupWithRoleAsync(UserRoleType userRoleType)
        //{
        //    try
        //    {
        //        var users = await _userManager.GetUsersInRoleAsync(userRoleType.ToDescription());

        //        var resultDto = new ListResultDto<UserLookupDto>(users
        //                                .Select(x => new UserLookupDto()
        //                                {
        //                                    Id = x.Id,
        //                                    FullName = StringHelper.GetUserDisplayFullName(x.Name, x.Surname, x.UserName)
        //                                }).ToList()); ;

        //        return resultDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "UserAppService > GetLookupWithRoleAsync ");
        //        return null;
        //    }
        //}

    }
}
