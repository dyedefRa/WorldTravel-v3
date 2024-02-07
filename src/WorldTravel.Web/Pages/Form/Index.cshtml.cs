using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Forms;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Form
{
    [AutoValidateAntiforgeryToken]
    public class IndexModel : WorldTravelPageModel
    {
        [BindProperty]
        public FormModel Form { get; set; }
        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Countries { get; set; }

        private readonly ILookupAppService _lookupAppService;
        private readonly IFormAppService _formAppService;

        public IndexModel(
               ILookupAppService lookupAppService,
               IFormAppService formAppService
               )
        {
            _lookupAppService = lookupAppService;
            _formAppService = formAppService;
        }
        private async Task LoadInitializeData()
        {
            Form = new FormModel();
            Genders = _lookupAppService.GetGenderLookup();
            Countries = await _lookupAppService.GetCountryLookupAsync();
        }

        public async Task<IActionResult> OnGet(bool returnPost = false)
        {
            return Redirect("~/Error?httpStatusCode=404");

            await LoadInitializeData();
            if (returnPost)
                Alerts.Info(L["FormRequestSuccessfully"].Value);
            else
               return Redirect("~/Error?httpStatusCode=404");

        }

        public async Task<IActionResult> OnPostAsync()
        {
            //return Redirect("~/Error?httpStatusCode=404");
            try
            {
                if (ModelState.IsValid)
                {
                    var input = ObjectMapper.Map<FormModel, CreateUpdateFormDto>(Form);
                    var addedResult = await _formAppService.CreateAsync(input);
                    if (addedResult != null)
                    {
                        return RedirectToAction("Index", "Form", new { returnPost = true });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Form > IndexModel > OnPostAsync has error! ");
            }

            Alerts.Danger(L["GeneralError"].Value);
            return Page();
        }

        public class FormModel
        {
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
            [Required]
            [SelectItems(nameof(Gender))]
            public GenderType Gender { get; set; }
            public Nullable<DateTime> BirthDate { get; set; }
            [Required]
            [SelectItems(nameof(Countries))]
            public int CountryId { get; set; }
            [TextArea(Rows = 3)]
            public string Description { get; set; }
        }
    }
}
