using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Messages.ViewModels;
using WorldTravel.Dtos.Users.ViewModels;

namespace WorldTravel.Web.Pages.Message
{
    [Authorize]
    //[AutoValidateAntiforgeryToken]
    public class IndexModel : WorldTravelPageModel
    {
        public AppUserViewModel UserViewModel { get; set; }
        public List<MessageViewModel> MessageViewModel { get; set; }


        private readonly IUserAppService _userAppService;
        private readonly IMessageAppService _messageAppService;

        public IndexModel(
            IUserAppService userAppService,
            IMessageAppService messageAppService
            )
        {
            _userAppService = userAppService;
            _messageAppService = messageAppService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return Redirect("~/Error?httpStatusCode=401");

                UserViewModel = (await _userAppService.GetAppUserAsync(CurrentUser.Id.Value)).Data;
                MessageViewModel = (await _messageAppService.GetUserMessageListAsync()).Data;

                return Page();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Account > Message > OnGetAsync  has error!");
                Alerts.Danger(L["GeneralError"].Value);

                return Page();
            }
        }
    }

}

//#region Method
//public async Task<JsonResult> OnPostUploadMessageContents(int messageId)
//{
//    try
//    {
//        var result = await _messageAppService.GetUserMessageWithContentListAsync(messageId);
//        if (result.Success)
//            return new JsonResult(new SuccessDataResult<List<MessageWithContentViewModel>>(result.Data));

//        return new JsonResult(new ErrorResult(result.Message));

//    }
//    catch (Exception ex)
//    {
//        Log.Error(ex, "Account > Message > OnPostUploadMessageContents  has error!");
//        Alerts.Danger(L["GeneralError"].Value);

//        return new JsonResult(new ErrorResult(L["GeneralError"].Value));
//    }
//}


//var formData = new FormData();
//formData.append('MessageId', messageId);
//$.ajax({
//    url: '/Message/Index?handler=UploadMessageContents',
//    type: "POST",
//    contentType: false,
//    processData: false,
//    data: formData,
//    beforeSend: function (xhr) {
//        xhr.setRequestHeader("RequestVerificationToken", $('input:hidden[name="__RequestVerificationToken"]').val());
//    },
//    success: function (result) {
//        if (result.success) {
//            $('#messageModalContent').empty();
//            $('#templateMessage').tmpl(result.data).appendTo('#messageModalContent');
//            $(".modal-body").animate({ scrollTop: 9999 }, "slow");
//        }
//        else {
//            toastr.error(result.message)
//        }
//    },
//    error: function (err) {
//        toastr.error('Bir hata oluştu.')
//    }
//});
//#endregion