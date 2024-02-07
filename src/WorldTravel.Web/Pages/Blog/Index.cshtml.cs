using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Jobs.ViewModels;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Dtos.Blog.ViewModels;
using WorldTravel.Dtos.Blog;

namespace WorldTravel.Web.Pages.Blog
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<BlogViewModel> Blog { get; set; }

        private readonly IBlogAppService _blogAppService;

        public IndexModel(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }
        public async Task<IActionResult> OnGet()
        {
           Blog = await _blogAppService.GetBlogListForAsync(new GetBlogRequestDto());
           return Page();
        }
    }
}
