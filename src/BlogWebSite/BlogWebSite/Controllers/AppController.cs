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
        public Task<PageData<BlogPost>> GetPostByAny(int pageIndex, int pageSize, string? keyword)
        {
            return appService.GetPostByAny(pageIndex, pageSize, keyword);
        }

        [HttpGet]
        public Task<PageData<BlogPost>> GetPostByCategory(int pageIndex, int pageSize, string? categorySlug)
        {
            return appService.GetPostByCategory(pageIndex, pageSize, categorySlug);
        }

        [HttpGet]
        public Task<PageData<BlogPost>> GetPostByTag(int pageIndex, int pageSize, string? tag)
        {
            return appService.GetPostByTag(pageIndex, pageSize, tag);
        }
    }
}
