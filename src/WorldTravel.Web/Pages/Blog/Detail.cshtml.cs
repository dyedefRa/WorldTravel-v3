using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldTravel.Abstract;
using WorldTravel.Dtos.Blog.ViewModels;
using WorldTravel.Dtos.Jobs.ViewModels;

namespace WorldTravel.Web.Pages.Blog
{
    [AutoValidateAntiforgeryToken]
    public class DetailModel : PageModel
    {
        [BindProperty]
        public BlogViewModel Blog { get; set; }
        

        private readonly IBlogAppService _blogAppService;

        public DetailModel(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }

        public async Task<IActionResult> OnGet(int id, bool r = false)
        {
            var data = await _blogAppService.GetBlogAsync(id);
            if (!data.Success)
                return Redirect("~/Error?httpStatusCode=404");

            Blog = data.Data;
          
            return Page();
        }
    }
}
