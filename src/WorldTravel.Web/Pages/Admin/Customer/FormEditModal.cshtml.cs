using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Forms;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using WorldTravel.Dtos.Forms.ViewModels;
using Volo.Abp.Identity.Web.Pages.Identity;
using System.Collections.Generic;
using WorldTravel.Dtos.Countries;
using WorldTravel.Enums;
using Microsoft.AspNetCore.Components;
using static IdentityServer4.Models.IdentityResources;
using System.Reflection;
using Volo.Abp;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using WorldTravel.Entities.Forms;
using Volo.Abp.ObjectMapping;

namespace WorldTravel.Web.Pages.Admin.Customer
{
    public class FormEditModalModel : IdentityPageModel
    {
        [BindProperty]
        public FormViewModel FormInfo { get; set; }

        public readonly IFormAppService _formAppService;

        public FormEditModalModel(IFormAppService formAppService)
        {
            _formAppService= formAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                FormInfo = ObjectMapper.Map<FormDto, FormViewModel>(await _formAppService.GetAsync(id));
                return Page();
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }
         
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ValidateModel();

                var formGet = await _formAppService.GetAsync(FormInfo.Id);
                formGet.FormIsOk = FormInfo.FormIsOk;
                formGet.BirthDate = DateTime.Now;
                var input = ObjectMapper.Map<FormDto, CreateUpdateFormDto>(formGet);
                await _formAppService.UpdateAsync(FormInfo.Id, input);

                return NoContent();
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }
          
        }

    }

}
