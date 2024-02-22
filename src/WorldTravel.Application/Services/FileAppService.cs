using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Files;
using WorldTravel.Enums;
using WorldTravel.Helpers;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Models.Results.Concrete;

namespace WorldTravel.Services
{
    public class FileAppService : CrudAppService<Entities.Files.File, FileDto, int, PagedAndSortedResultRequestDto, FileDto, FileDto>, IFileAppService
    {
        public FileAppService(IRepository<Entities.Files.File, int> repository) : base(repository)
        {
        }

        public async Task<IDataResult<FileDto>> SaveFileAsync(IFormFile fromFile, UploadType uploadType, FileType fileType = FileType.Image)
        {
            try
            {
                var uploadResult = await UploadHelper.UploadAsync(fromFile, uploadType);
                var defaultFileDto = uploadResult.Data;
                defaultFileDto.FileType = fileType;
                var entity = ObjectMapper.Map<FileDto, Entities.Files.File>(defaultFileDto);
                var insertedData = await Repository.InsertAsync(entity, true);
                var resultDto = ObjectMapper.Map<Entities.Files.File, FileDto>(insertedData);
                return new SuccessDataResult<FileDto>(resultDto);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "FileAppService > SaveFileAsync");
                return new ErrorDataResult<FileDto>(new FileDto(), "Dosya eklenirken sorun oluştu.");
            }
        }

        public async Task<IDataResult<string>> DeleteFileAsync(int id, bool deleteInServer = true)
        {
            try
            {
                if (deleteInServer)
                {
                    var file = await Repository.GetAsync(id);
                    if (file != null)
                    {
                        UploadHelper.Delete(file.FullPath);
                    }
                }

                await Repository.DeleteAsync(id);
                //await SoftDeleteAsync(id);
                return new SuccessDataResult<string>();

            }
            catch (Exception ex)
            {
                Log.Error(ex, "FileAppService > DeleteFileAsync");
                return new ErrorDataResult<string>("Hata oluştu.");
            }
        }

        public string SetDefaultImageIfFileIsNull(int? imageId, GenderType? genderType = GenderType.Male, bool isAdmin = false)
        {
            try
            {
                if (imageId.HasValue)
                {
                    var image = Repository.FindAsync(imageId.Value).Result;
                    if (image != null)
                    {
                        return image.Path;
                    }
                }

                if (isAdmin)
                {
                    return WorldTravelConsts.DEFAULT.AdminAvatarImageUrl;
                }
                else if (genderType == GenderType.Female)
                {
                    return WorldTravelConsts.DEFAULT.FemaleAvatarImageUrl;
                }
                else
                {
                    return WorldTravelConsts.DEFAULT.MaleAvatarImageUrl;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "FileAppService > SetDefaultImageIfFileNullAsync");
                return "";
            }
        }


        //Company EDİT kullandık mı?
        //public async Task<IDataResult<bool>> UploadCompanyFileAndFillCompanyDtoAsync(CreateUpdateCompanyDto createUpdateCompanyDto, IFormFile file, FileType fileType)
        //{
        //    try
        //    {
        //        CompanyFileDto companyFileDto = new CompanyFileDto();

        //        var data = await UploadHelper.UploadAsync(file, UploadType.Company);
        //        var uploadedFileDto = data.Data;
        //        uploadedFileDto.FileType = fileType;
        //        companyFileDto.File = uploadedFileDto;
        //        createUpdateCompanyDto.CompanyFiles.Add(companyFileDto);
        //        return new SuccessDataResult<bool>(true);

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "FileAppService > SaveCompanyFileAndRelationAsync");
        //        return new ErrorDataResult<bool>(false);
        //    }
        //}


        //public async Task<IDataResult<GetFileRequestDto>> GetFileAsync(int fileId)
        //{
        //    try
        //    {
        //        var file = await Repository.GetAsync(fileId);
        //        if (file == null)
        //        {
        //            //burada standart bir resim dönülebilir.
        //            return new ErrorDataResult<GetFileRequestDto>("Dosya bulunamadı.");

        //        }
        //        Byte[] fileBytes = System.IO.File.ReadAllBytes(file.FullPath);

        //        return new SuccessDataResult<GetFileRequestDto>(new GetFileRequestDto
        //        {
        //            FileBytes = fileBytes,
        //            ContentType = file.ContentType
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "FileAppService > GetFileAsync");
        //        return new ErrorDataResult<GetFileRequestDto>();
        //    }
        //}

        public async Task SoftDeleteAsync(int Id)
        {
            var entity = Repository.FirstOrDefault(x => x.Id == Id);
            entity.Status = Enums.Status.Deleted;
            await Repository.UpdateAsync(entity);
        }

        public async Task<string> GetFilePathByFileId(int fileId)
        {
            var file = await Repository.FirstOrDefaultAsync(x => x.Id == fileId);
            if (file != null)
            {
                return file.Path;
            }

            return ""; // Buraya default resim.
        }
    }
}
