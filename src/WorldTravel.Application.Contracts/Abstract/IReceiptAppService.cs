using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WorldTravel.Dtos.Receipts;

namespace WorldTravel.Abstract
{
    public interface IReceiptAppService : ICrudAppService<ReceiptDto, int, PagedAndSortedResultRequestDto, CreateUpdateReceiptDto, CreateUpdateReceiptDto>
    {
        Task<List<ReceiptViewModel>> GetReceiptByUserIdAsync(Guid UserId);
        Task<bool> CreateManualAsync(ReceiptDto receiptDto);
    }
}
