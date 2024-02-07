using WorldTravel.Dtos.Files;
using WorldTravel.Enums;
using WorldTravel.Models.Results.Abstract;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WorldTravel.Abstract
{
    public interface IFileAppService : ICrudAppService<FileDto, int, PagedAndSortedResultRequestDto, FileDto, FileDto>
    {
        Task<IDataResult<FileDto>> SaveFileAsync(IFormFile fromFile, UploadType uploadType, FileType fileType = FileType.Image);
        Task<IDataResult<string>> DeleteFileAsync(int id, bool deleteInServer = true);
        string SetDefaultImageIfFileIsNull(int? imageId, GenderType genderType);
        //Task<IDataResult<GetFileRequestDto>> GetFileAsync(int fileId);
        //Task<IDataResult<bool>> UploadCompanyFileAndFillCompanyDtoAsync(CreateUpdateCompanyDto createUpdateCompanyDto, IFormFile file, FileType fileType);
        Task SoftDeleteAsync(int Id);
        Task<string> GetFilePathByFileId(int fileId);
    }
}
