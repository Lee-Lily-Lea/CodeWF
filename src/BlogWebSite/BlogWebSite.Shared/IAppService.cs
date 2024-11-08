using BlogWebSite.Shared.Models;
using WebApiClientCore.Attributes;
using static BlogWebSite.Shared.ApiRoutes;

namespace BlogWebSite.Shared
{
    [LoggingFilter]
    public interface IAppService
    {
        [HttpGet(AppService + nameof(GetPostByAny))]
        Task<PageData<BlogPost>> GetPostByAny(int pageIndex, int pageSize, string? keyword);

        [HttpGet(AppService + nameof(GetPostByCategory))]
        Task<PageData<BlogPost>> GetPostByCategory(int pageIndex, int pageSize, string? categorySlug);

        [HttpGet(AppService + nameof(GetPostByTag))]
        Task<PageData<BlogPost>> GetPostByTag(int pageIndex, int pageSize, string? tag);

        [HttpGet(AppService + nameof(GetPostByAuthor))]
        Task<PageData<BlogPost>> GetPostByAuthor(int pageIndex, int pageSize, string? author);
    }


}
