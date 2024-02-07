using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Receipts;
using WorldTravel.Entities.Receipts;
using WorldTravel.Permissions;

namespace WorldTravel.Services
{
    [Authorize(WorldTravelPermissions.Receipt.Default)]
    public class ReceiptAppService : CrudAppService<Receipt, ReceiptDto, int, PagedAndSortedResultRequestDto, CreateUpdateReceiptDto, CreateUpdateReceiptDto>, IReceiptAppService
    {
        public ReceiptAppService(IRepository<Receipt, int> repository) : base(repository)
        {

        }

        [AllowAnonymous]
        public async Task<List<ReceiptViewModel>> GetReceiptByUserIdAsync(Guid UserId)
        {
            try
            {
                var result = new List<ReceiptViewModel>();

                var query = Repository.Where(x => x.UserId == UserId).AsQueryable();
                query = query.Include(x => x.File);
                query = query.Include(x => x.User);

                //input.Sorting = "id desc";

                //query = ApplySorting(query, input);
                //query = ApplyPaging(query, input);

                var list = await query.ToListAsync();
                var viewModels = list.Select(x =>
                {
                    var viewModel = ObjectMapper.Map<Receipt, ReceiptViewModel>(x);
                    viewModel.FileId = x.FileId;
                    viewModel.UserId = x.UserId;
                    viewModel.File.Name = x.File.Name;
                    viewModel.File.FileType = x.File.FileType;
                    viewModel.File.Size = x.File.Size;
                    viewModel.File.FullPath = x.File.FullPath;
                    viewModel.File.CreatedDate = x.File.CreatedDate;
                    return viewModel;
                }).ToList();

                result.AddRange(viewModels);
                return result;
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }
        }


        [AllowAnonymous]
        public async Task<bool> CreateManualAsync(ReceiptDto receiptDto)
        {
            try
            {
                var result = ObjectMapper.Map<ReceiptDto, Receipt>(receiptDto);
                await Repository.InsertAsync(result);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
