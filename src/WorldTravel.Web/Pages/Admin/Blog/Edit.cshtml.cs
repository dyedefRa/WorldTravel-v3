using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Volo.Abp.Identity.Web.Pages.Identity;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Sliders.ViewModels;
using WorldTravel.Dtos.Sliders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Components;
using WorldTravel.Enums;
using WorldTravel.Services;
using WorldTravel.Dtos.Blog;

namespace WorldTravel.Web.Pages.Admin.Blog
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EditModel : WorldTravelPageModel
    {

        [Parameter]
        public int Id { get; set; }

        [BindProperty]
        public EditBlogModel BlogInfo { get; set; }

        public readonly IBlogAppService _blogService;
        private readonly IFileAppService _fileAppService;

        public EditModel(IBlogAppService blogService, IFileAppService fileAppService)
        {
            _blogService = blogService;
            _fileAppService = fileAppService;
        }

        public async Task OnGetAsync(int id)
        {
            try
            {
                Id = id;
                var sliderDto = await _blogService.GetAsync(Id);
                BlogInfo = ObjectMapper.Map<BlogDto, EditBlogModel>(sliderDto);
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }

        }

        public virtual async Task<IActionResult> OnPostAsync(int Id)
        {
            try
            {
                ValidateModel();
                var formGet = await _blogService.GetAsync(Id);
                formGet.Description = BlogInfo.Description;
                formGet.Title = BlogInfo.Title;
                formGet.IsSeenHomePage = BlogInfo.IsSeenHomePage;
                formGet.SeoDescription = BlogInfo.SeoDescription;
                formGet.SeoTitle = BlogInfo.SeoTitle;

                if (BlogInfo.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(BlogInfo.MainImage, UploadType.Job);
                    if (fileResult.Success)
                    {
                        formGet.ImageId = fileResult.Data.Id;
                    }
                }

                var input = ObjectMapper.Map<BlogDto, CreateUpdateBlogDto>(formGet);
                await _blogService.UpdateAsync(Id, input);
                return NoContent();
            }
            catch (Exception ex)
            {
                var ss = ex.ToString();
                throw;
            }

        }

        public class EditBlogModel
        {
            [HiddenInput]
            public int Id { get; set; }

            public string Title { get; set; }

            public string SeoTitle { get; set; }

            [TextArea(Rows = 2)]
            public string SeoDescription { get; set; }
          
            [TextArea(Rows = 5)]
            public string Description { get; set; }

            public IFormFile MainImage { get; set; }

            public int Rank { get; set; }
            public bool IsSeenHomePage { get; set; }
        }


    }

}
