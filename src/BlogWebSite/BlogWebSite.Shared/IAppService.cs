using BlogWebSite.Shared.Models;

using WebApiClientCore.Attributes;

namespace BlogWebSite.Shared
{
    [LoggingFilter]
    public interface IAppService
    {
        [HttpGet("api/app/GetPostByCategory")]
        Task<PageData<BlogPost>> GetPostByCategory(int pageIndex, int pageSize, string categorySlug, string? key);
    }
}
