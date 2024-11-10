using BlogWebSite.Shared;
using BlogWebSite.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppController(IAppService appService) : ControllerBase, IAppService
    {
        [HttpGet]
        public Task<List<CategoryItem>> GetAllCategoryItems()
        {
            return appService.GetAllCategoryItems();
        }

        [HttpGet]
        public Task<PageData<BlogPost>> GetPostByAny(int pageIndex, int pageSize, string? keyword)
        {
            return appService.GetPostByAny(pageIndex, pageSize, keyword);
        }

        [HttpGet]
        public Task<PageData<BlogPost>> GetPostByAuthor(int pageIndex, int pageSize, string? author)
        {
            return appService.GetPostByAuthor(pageIndex, pageSize, author);
        }

        [HttpGet]
        public Task<PageData<BlogPost>> GetPostByCatSlug(int pageIndex, int pageSize, string? categorySlug)
        {
            return appService.GetPostByCatSlug(pageIndex, pageSize, categorySlug);
        }

        [HttpGet]
        public Task<BlogPost?> GetPostBySlug(string slug)
        {
            return appService.GetPostBySlug(slug);
        }

        [HttpGet]
        public Task<PageData<BlogPost>> GetPostByTag(int pageIndex, int pageSize, string? tag)
        {
            return appService.GetPostByTag(pageIndex, pageSize, tag);
        }
    }
}
