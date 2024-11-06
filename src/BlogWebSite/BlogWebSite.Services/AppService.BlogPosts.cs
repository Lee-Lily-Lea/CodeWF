using BlogWebSite.Shared;
using BlogWebSite.Shared.Models;
using YamlDotNet.Core.Tokens;

namespace BlogWebSite.Services
{
    public partial class AppService
    {
        #region BaseQuery
        private IEnumerable<BlogPost> GetPostByTagCore(IEnumerable<BlogPost>? source, string? tag)
        {
            if (source is null)
            {
                return [];
            }

            if (string.IsNullOrEmpty(tag))
            {
                return source;
            }

            return source.Where(p => p.Tags?.Contains(tag) == true);
        }

        private IEnumerable<BlogPost> GetPostByAnyCore(IEnumerable<BlogPost>? source, string? keyword)
        {
            if (source is null)
            {
                return [];
            }

            if (string.IsNullOrEmpty(keyword))
            {
                return source;
            }

            var posts = source.Where(p =>
                p.Title?.Contains(keyword) == true
                || p.Description?.Contains(keyword) == true
                || p.Slug?.Contains(keyword) == true
                || p.Author?.Contains(keyword) == true
                || p.LastModifyUser?.Contains(keyword) == true
                || p.Content?.Contains(keyword) == true
            );

            return posts;
        }

        private IEnumerable<BlogPost> GetPostByCategoryCore(IEnumerable<BlogPost>? source, string? categorySlug)
        {
            if (source is null)
            {
                return [];
            }

            if (string.IsNullOrEmpty(categorySlug))
            {
                return source;
            }

            return source.Where(p => p.Categories?.Contains(categorySlug) == true);
        }
        #endregion




        Task<PageData<BlogPost>> IAppService.GetPostByAny(int pageIndex, int pageSize, string? keyword)
        {
            var posts = GetPostByAnyCore(_blogPosts, keyword);

            var total = posts.Count();
            var data = posts
                .OrderByDescending(post => post.Lastmod)
                .ThenByDescending(post => post.Date)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Task.FromResult(new PageData<BlogPost>(pageIndex, pageSize, total, data));
        }

        Task<PageData<BlogPost>> IAppService.GetPostByTag(int pageIndex, int pageSize, string? tag)
        {
            if (pageIndex >= 4)
            {

            }

            var posts = GetPostByTagCore(_blogPosts, tag);

            var total = posts.Count();
            var data = posts
                .OrderByDescending(post => post.Lastmod)
                .ThenByDescending(post => post.Date)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            return Task.FromResult(new PageData<BlogPost>(pageIndex, pageSize, total, data));
        }

        Task<PageData<BlogPost>> IAppService.GetPostByCategory(int pageIndex, int pageSize, string? categorySlug)
        {
            var posts = GetPostByCategoryCore(_blogPosts, categorySlug);

            var total = posts.Count();
            var data = posts
                .OrderByDescending(post => post.Lastmod)
                .ThenByDescending(post => post.Date)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Task.FromResult(new PageData<BlogPost>(pageIndex, pageSize, total, data));
        }
    }
}
