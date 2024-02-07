using WorldTravel.Dtos.Files;
using WorldTravel.Enums;
using WorldTravel.Extensions;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Models.Results.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WorldTravel.Helpers
{
    public static class UploadHelper
    {
        public static async Task<IDataResult<FileDto>> UploadAsync(IFormFile file, UploadType uploadType)
        {
            if (file.Length <= 0)
                return new ErrorDataResult<FileDto>("Dosya boş.");

            string relatedSettingFolder = $"wwwroot/uploads/{EnumExtensions.GetEnumDescription(uploadType)}/";

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), relatedSettingFolder);
            string fileName = GenerateFileName(file.FileName);
            string filePath = Path.Combine(uploads, fileName);

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var media = new FileDto();
            media.Name = fileName;
            media.Path = Path.Combine(relatedSettingFolder.ToFileShownPath(), fileName);
            media.FullPath = filePath;
            media.Size = file.Length;
            media.Status = Status.Active;
            media.CreatedDate = DateTime.Now;
            media.Rank = 1;
            return new SuccessDataResult<FileDto>(media);
        }

        public static IDataResult<string> Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);

                return new SuccessDataResult<string>();
            }

            return new ErrorDataResult<string>("Dosya yok.");
        }

        private static string GenerateFileName(string fileName) => string.Format($"{Guid.NewGuid()}_{DateTime.Now.ToString("dd-M-yyyy-HH-mm-ss")}{Path.GetExtension(fileName)}");

    }
}
