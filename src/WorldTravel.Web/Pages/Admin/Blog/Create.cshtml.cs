using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Blog;
using WorldTravel.Enums;

namespace WorldTravel.Web.Pages.Admin.Blog
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CreateModel : WorldTravelPageModel
    {
        [BindProperty]
        public CreateBlogModel Blog { get; set; }

        private readonly IBlogAppService _blogAppService;
        private readonly IFileAppService _fileAppService;

        public CreateModel(
               IBlogAppService blogAppService,
               IFileAppService fileAppService
               )
        {
            _blogAppService = blogAppService;
            _fileAppService = fileAppService;
        }

        public async Task OnGet()
        {
            Blog = new CreateBlogModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var createUpdateDto = ObjectMapper.Map<CreateBlogModel, CreateUpdateBlogDto>(Blog);
                List<int> newFileIds = new List<int>();

                if (Blog.MainImage != null)
                {
                    var fileResult = await _fileAppService.SaveFileAsync(Blog.MainImage, UploadType.Job);
                    if (fileResult.Success)
                    {
                        createUpdateDto.ImageId = fileResult.Data.Id;
                    }
                }

                var addedResult = await _blogAppService.CreateAsync(createUpdateDto);
                if (addedResult != null)
                {

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Admin > Blog > Create > OnPostAsync");
            }

            return NoContent();
        }

        public class CreateBlogModel
        {
            [Required]
            public string Title { get; set; }

            [Required]
            public string SeoTitle { get; set; }

            [Required]
            [TextArea(Rows = 2)]
            public string SeoDescription { get; set; }
            [Required]
            [TextArea(Rows = 5)]
            public string Description { get; set; }
           
            [Required]
            public IFormFile MainImage { get; set; }
          
            public int Rank { get; set; }
            public bool IsSeenHomePage { get; set; }
        }
    }
}
