using BlogWebSite.Services;
using BlogWebSite.Shared;
using BlogWebSite.Shared.Models;

using Microsoft.AspNetCore.Mvc;


namespace BlogWebSite.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppController(AppService appService) : ControllerBase, IAppService
    {
        [HttpGet]
        public Task<PageData<BlogPost>> GetPostByCategory(int pageIndex, int pageSize, string categorySlug, string? key)
        {
            return appService.GetPostByCategory(pageIndex, pageSize, categorySlug, key);
        }
    }
}
