using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using WorldTravel.Abstract;
using WorldTravel.Dtos.CountryContents;
using WorldTravel.Dtos.CountryContents.ViewModels;
using WorldTravel.Dtos.Files.ViewModels;
using WorldTravel.Entities.CountryContentFiles;
using WorldTravel.Entities.CountryContents;
using WorldTravel.Entities.Files;
using WorldTravel.Enums;
using WorldTravel.Localization;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Models.Results.Concrete;
using WorldTravel.Permissions;

namespace WorldTravel.Services
{
    [Authorize(WorldTravelPermissions.CountryContent.Default)]
    public class CountryContentAppService : CrudAppService<CountryContent, CountryContentDto, int, PagedAndSortedResultRequestDto, CreateUpdateCountryContentDto, CreateUpdateCountryContentDto>, ICountryContentAppService
    {
        private readonly IStringLocalizer<WorldTravelResource> _L;
        private readonly IRepository<CountryContentFile, int> _countryContentFileRepository;

        public CountryContentAppService(
            IRepository<CountryContent, int> repository,
            IStringLocalizer<WorldTravelResource> L,
            IRepository<CountryContentFile, int> countryContentFileRepository
            ) : base(repository)
        {
            _L = L;
            _countryContentFileRepository = countryContentFileRepository;
        }

        public async Task<PagedResultDto<CountryContentViewModel>> GetCountryContentListAsync(GetCountryContentRequestDto input)
        {
            var result = new PagedResultDto<CountryContentViewModel>();

            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Country)
                .Include(x => x.Image)
                .Include(x => x.CountryContentFiles).ThenInclude(x => x.File);

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), x => x.Country.Title.Contains(input.CountryNameFilter));

            var totalCount = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                var files = x.CountryContentFiles.Select(x => x.File).ToList();
                var viewModel = ObjectMapper.Map<CountryContent, CountryContentViewModel>(x);
                viewModel.CountryName = x.Country.Title;
                viewModel.PreviewImageUrl = x.Image.Path;
                if (files != null)
                {
                    viewModel.TotalImageCount = files.Where(x => x.FileType == FileType.Image).Count();
                    viewModel.TotalVideoCount = files.Where(x => x.FileType == FileType.Video).Count();
                }
                return viewModel;
            }).ToList();

            result.Items = viewModels;
            result.TotalCount = totalCount;
            return result;
        }


        [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
        public async Task<IDataResult<bool>> AddOrRemoveCountryContentFileRelationAsync(int countryContentId, List<int> newFileIds, List<int> removedFileIds = null, bool isShareContent = false)
        {
            try
            {
                if (removedFileIds != null && removedFileIds.Count > 0)
                {
                    foreach (var removedFileId in removedFileIds)
                    {
                        var removedEntity = _countryContentFileRepository.FirstOrDefault(x => x.CountryContentId == countryContentId && x.FileId == removedFileId);
                        await _countryContentFileRepository.DeleteAsync(removedEntity, true);
                    }
                }

                if (newFileIds.Count > 0)
                {
                    await _countryContentFileRepository.InsertManyAsync(
                        newFileIds.Select(x => new CountryContentFile() { CountryContentId = countryContentId, FileId = x, IsShareContent = isShareContent }), true);
                }

                return new SuccessDataResult<bool>(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CountryContentAppService > AddOrRemoveCountryContentFileRelationAsync");
                return new ErrorDataResult<bool>(false, L["GeneralError"].Value);
            }
        }

        public async Task SoftDeleteAsync(int Id)
        {
            var entity = Repository.FirstOrDefault(x => x.Id == Id);
            entity.Status = Enums.Status.Deleted;
            await Repository.UpdateAsync(entity);
        }

        #region User Pages

        /// <summary>
        /// Web tarafındaki Country/Index sayfasında kullanılır.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<List<CountryContentViewModel>> GetCountryContentListForUserAsync(GetCountryContentRequestDto input)
        {
            var result = new List<CountryContentViewModel>();
            var query = Repository.Where(x => x.Status == Status.Active).AsQueryable();
            query = query
                .Include(x => x.Country)
                .Include(x => x.Image);
            //.Include(x => x.CountryContentFiles).ThenInclude(x => x.File);

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), x => x.Country.Title.Contains(input.CountryNameFilter))
                .WhereIf(input.IsSeenHomePage == true, x => x.IsSeenHomePage == true);

            //var totalCount = await query.CountAsync();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var list = await query.ToListAsync();
            var viewModels = list.Select(x =>
            {
                //var files = x.CountryContentFiles.Select(x => x.File).ToList();
                var viewModel = ObjectMapper.Map<CountryContent, CountryContentViewModel>(x);
                viewModel.CountryName = x.Country.Title;
                viewModel.PreviewImageUrl = x.Image.Path;
                //if (files != null)
                //{
                //    viewModel.TotalImageCount = files.Where(x => x.FileType == FileType.Image).Count();
                //    viewModel.TotalVideoCount = files.Where(x => x.FileType == FileType.Video).Count();
                //}
                return viewModel;
            }).OrderBy(x => x.Rank).ToList();

            return viewModels;
        }

        [AllowAnonymous]
        public async Task<IDataResult<CountryContentViewModel>> GetCountryContentAsync(int countryContentId)
        {
            try
            {
                var data = await Repository
                    .Include(x => x.Country)
                    .Include(x => x.Image)
                    .Include(x => x.CountryContentFiles).ThenInclude(x => x.File)
                    .Where(x => x.Status == Status.Active)
                    .FirstOrDefaultAsync(x => x.Id == countryContentId);

                if (data == null)
                    return new ErrorDataResult<CountryContentViewModel>(L["EntityNotFoundError"]);

                var result = ObjectMapper.Map<CountryContent, CountryContentViewModel>(data);
                result.CountryName = data.Country.Title;
                result.PreviewImageUrl = data.Image.Path;
                result.CountryId = data.CountryId;
                var files = data.CountryContentFiles.Select(x => x.File).ToList();
                if (files != null)
                {
                    result.ImageFiles = ObjectMapper.Map<List<File>, List<FileViewModel>>(files.Where(x => x.FileType == FileType.Image).ToList());
                    result.VideoFiles = ObjectMapper.Map<List<File>, List<FileViewModel>>(files.Where(x => x.FileType == FileType.Video).ToList());
                }

                return new SuccessDataResult<CountryContentViewModel>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CountryContentAppService > GetCountryContentAsync");

                return new ErrorDataResult<CountryContentViewModel>(L["GeneralError"]);
            }
        }

        #endregion

    }
}
