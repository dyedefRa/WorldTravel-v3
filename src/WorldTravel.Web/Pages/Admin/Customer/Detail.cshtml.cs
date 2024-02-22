using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity.Web.Pages.Identity;
using Volo.Abp.Users;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Forms;
using WorldTravel.Dtos.Forms.ViewModels;
using WorldTravel.Dtos.Messages.ViewModels;
using WorldTravel.Dtos.Receipts;
using WorldTravel.Dtos.Users.ViewModels;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin.Customer
{
    public class Detail : PageModel
    {
        private readonly IUserAppService _userAppService;
        private readonly IReceiptAppService _receiptAppService;

        public AppUserViewModel UserViewModel { get; set; }
        public List<ReceiptViewModel> Receipts { get; set; }

        public Detail(
            IUserAppService userAppService,
            IReceiptAppService receiptAppService
            )
        {
            _userAppService = userAppService;
            _receiptAppService = receiptAppService;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var user = (await _userAppService.GetAppUserAsync(id));
            if (!user.Success || user.Data == null)
                return Redirect("~/Error?httpStatusCode=404");

            UserViewModel = user.Data;
            Receipts = await _receiptAppService.GetReceiptByUserIdAsync(id);

            return Page();
        }
    }
}
