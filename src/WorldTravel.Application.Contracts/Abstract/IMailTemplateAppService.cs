using WorldTravel.Dtos.MailTemplates;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WorldTravel.Abstract
{
    public interface IMailTemplateAppService : ICrudAppService<MailTemplateDto, int, PagedAndSortedResultRequestDto, MailTemplateDto, MailTemplateDto>
    {
        Task<bool> SendNewPasswordMailAsync(string passwordChangeUrl, string toMail);
    }
}
