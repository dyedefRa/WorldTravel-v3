using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Pages.Identity;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Forms.ViewModels;
using WorldTravel.Dtos.Receipts;
using WorldTravel.Dtos.Users.ViewModels;
using WorldTravel.Enums;
using WorldTravel.Models.Pages.Account;
using WorldTravel.Models.Results.Concrete;

namespace WorldTravel.Web.Pages.Account
{
    [AutoValidateAntiforgeryToken]
    public class ManageModel : IdentityPageModel
    {
        [BindProperty]
        public UserManageModel UserManageInputModel { get; set; }
        private readonly IFormAppService _formAppService;
        private readonly IUserAppService _userAppService;
        private readonly IReceiptAppService _receiptAppService;
        private readonly IFileAppService _fileAppService;
        private readonly IIdentityUserAppService _identityUserAppService;

        [BindProperty]
        public List<FormViewModel> Forms { get; set; }

        [BindProperty]
        public List<ReceiptViewModel> Receipts { get; set; }

        [BindProperty]
        public ReceiptModel Receipt { get; set; }

        public ManageModel(
            IUserAppService userAppService,
            IFormAppService formAppService,
            IReceiptAppService receiptAppService,
            IFileAppService fileAppService,
            IIdentityUserAppService identityUserAppService
            )
        {
            _userAppService = userAppService;
            _formAppService = formAppService;
            _receiptAppService = receiptAppService;
            _fileAppService = fileAppService;
            _identityUserAppService = identityUserAppService;

        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return Redirect("~/Error?httpStatusCode=401");

                var currentUser = await _userAppService.GetAppUserAsync(CurrentUser.Id.Value);
                var user = await _identityUserAppService.GetAsync(CurrentUser.Id.Value);

                UserManageInputModel = ObjectMapper.Map<AppUserViewModel, UserManageModel>(currentUser.Data);
                Forms = await _formAppService.GetFormListAsyncUserId(CurrentUser.Id.Value);
                Receipts = await _receiptAppService.GetReceiptByUserIdAsync(CurrentUser.Id.Value);

                if (Forms != null && Forms.Count > 0)
                {
                    bool formVisaIsHave = Forms.Where(x => x.FormRegType == "Vize Başvurusu").Any();
                    if (formVisaIsHave == false)
                    {
                        UserManageInputModel.ProfileIsOk = 0;
                    }

                    double totalPercent = 0;
                    List<double> yuzdeler = new List<double>();
                    foreach (var item in Forms.Where(x => x.FormRegType == "Vize Başvurusu"))
                    {
                        totalPercent = 0;
                        yuzdeler = new List<double>();
                        foreach (var item2 in Forms.Where(x => x.FormRegType == "Vize Başvurusu"))
                        {
                            yuzdeler.Add(item2.FormIsOk);
                        }

                        foreach (var yuzde in yuzdeler)
                        {
                            totalPercent += yuzde / 100;
                        }

                        double ortalamaYuzde = totalPercent / yuzdeler.Count * 100;
                        UserManageInputModel.ProfileIsOk = Convert.ToInt32(ortalamaYuzde);
                    }

                }
                else
                {
                    UserManageInputModel.ProfileIsOk = 0;
                }

                return Page();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Account > Manage > OnGetAsync  has error!");
                Alerts.Danger(L["GeneralIdentityError"].Value);

                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ManageProfileModel input = new ManageProfileModel();
                    input.Name = UserManageInputModel.Name;
                    input.Surname = UserManageInputModel.Surname;
                    input.Email = UserManageInputModel.Email;
                    input.PhoneNumber = UserManageInputModel.PhoneNumber;
                    input.BirthDate = UserManageInputModel.BirthDate;

                    var result = await _userAppService.ManageProfileAsync(CurrentUser.Id.Value, input);
                    if (result.Success)
                        return RedirectToAction("Manage", "Account");

                }

                Alerts.Danger(L["GeneralIdentityError"].Value);

                return Page();

            }
            catch (AbpIdentityResultException ex)
            {
                var identityError = ex.IdentityResult;
                if (!identityError.Succeeded && identityError.Errors.Any())
                {
                    var totalErrorCount = identityError.Errors.Count();
                    if (identityError.Errors.Any(x => x.Code == "DuplicateUserName"))
                    {
                        Alerts.Danger($"'{UserManageInputModel.UserName}' kullanıcı adına sahip bir kullanıcı zaten var.");
                        totalErrorCount--;
                    }
                    if (identityError.Errors.Any(x => x.Code == "DuplicateEmail"))
                    {
                        Alerts.Danger($"'{UserManageInputModel.Email}' mailine sahip bir kullanıcı zaten var.");
                        totalErrorCount--;
                    }
                    if (totalErrorCount > 0)
                    {
                        Alerts.Danger(L["GeneralIdentityError"].Value);
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CompanyRegisterModel > OnPostAsync has error! ");
                Alerts.Danger(L["GeneralIdentityError"].Value);

                return Page();
            }
        }

        public async Task<JsonResult> OnPostChangeProfileImage(IFormFile image)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return new JsonResult(new ErrorResult(L["GeneralUploadError"].Value));

                var result = await _userAppService.ChangeProfileImageAsync(CurrentUser.Id.Value, image);
                if (result.Success)
                    return new JsonResult(new SuccessResult());

                return new JsonResult(new ErrorResult(result.Data));

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Account > Manage > OnGetAsync  has error!");
                Alerts.Danger(L["GeneralUploadError"].Value);

                return new JsonResult(new ErrorResult(L["GeneralUploadError"].Value));
            }
        }

        public async Task<JsonResult> OnPostInsertReceiptFile(IFormFile image)
        {
            try
            {
                if (image != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(image, UploadType.CountryContent);
                    if (fileResult.Success)
                    {
                        if (!CurrentUser.IsAuthenticated)
                            return new JsonResult(new ErrorResult(L["GeneralUploadError"].Value));

                        var receipt = new ReceiptDto()
                        {
                            FileId = fileResult.Data.Id,
                            ReceiptOk = false,
                            UserId = (Guid)CurrentUser.Id,
                        };

                        //var result = ObjectMapper.Map<ReceiptDto, CreateUpdateReceiptDto>(receipt);
                        var addedResult = await _receiptAppService.CreateManualAsync(receipt); //

                        if (addedResult)
                        {
                            return new JsonResult(new SuccessResult());
                        }
                        else
                        {
                            return new JsonResult(new ErrorResult(L["GeneralUploadError"].Value));
                        }

                    }
                    else
                    {
                        return new JsonResult(new ErrorResult(L["GeneralUploadError"].Value));
                    }
                }
                else
                {
                    return new JsonResult(new ErrorResult(L["GeneralUploadError"].Value));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Account > Manage > OnGetAsync  has error!");
                return new JsonResult(new ErrorResult(L["GeneralUploadError"].Value));

            }
        }

        public class ReceiptModel
        {
            public int FileId { get; set; }
            public Guid UserId { get; set; }
            public bool ReceiptIsOk { get; set; }
        }


        public class UserManageModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            [DisabledInput]
            public string UserName { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Surname { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [Phone]
            [StringLength(16, MinimumLength = 16, ErrorMessage = "Telefon alanı geçerli bir telefon numarası olmalıdır.")]
            public string PhoneNumber { get; set; }
            public DateTime CreationTime { get; set; }
            [DisabledInput]
            public GenderType Gender { get; set; }
            public DateTime? BirthDate { get; set; }
            public int? ProfileIsOk { get; set; }
            public string ImageUrl { get; set; }


        }
    }
}
