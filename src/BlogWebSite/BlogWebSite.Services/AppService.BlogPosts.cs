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

            return source.Where(p =>
                p.Tags?.Any(x => x.Contains(tag, StringComparison.OrdinalIgnoreCase)) is true
            );
        }

        private IEnumerable<BlogPost> GetPostByAuthorCore(IEnumerable<BlogPost>? source, string? author)
        {
            if (source is null)
            {
                return [];
            }

            if (string.IsNullOrEmpty(author))
            {
                return source;
            }

            if (author is BlogPost.DefaultAuthor)
            {
                return source.Where(p => p.Author == null);
            }
            else
            {
                return source.Where(p => p.Author?.Contains(author) == true);
            }
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

        private IEnumerable<BlogPost> GetPostByCatSlugCore(IEnumerable<BlogPost>? source, string? categorySlug)
        {
            if (source is null)
            {
                return [];
            }

            if (string.IsNullOrEmpty(categorySlug))
            {
                return source;
            }

            return source.Where(
                p => p.Categories?.Any(x => x.Contains(categorySlug, StringComparison.OrdinalIgnoreCase)) is true
                );
        }

        private List<BlogPost> BlogPostToListCore(IEnumerable<BlogPost> source, int pageIndex, int pageSize)
        {
            var posts = source
                .OrderByDescending(post => post.Lastmod)
                .ThenByDescending(post => post.Date)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return posts;
        }
        #endregion



        Task<PageData<BlogPost>> IAppService.GetPostByTag(int pageIndex, int pageSize, string? tag)
        {
            var posts = GetPostByTagCore(_blogPosts, tag);

            var total = posts.Count();
            var data = BlogPostToListCore(posts, pageIndex, pageSize);

            return Task.FromResult(new PageData<BlogPost>(pageIndex, pageSize, total, data));
        }

        Task<PageData<BlogPost>> IAppService.GetPostByAuthor(int pageIndex, int pageSize, string? author)
        {
            var posts = GetPostByAuthorCore(_blogPosts, author);

            var total = posts.Count();
            var data = BlogPostToListCore(posts, pageIndex, pageSize);

            return Task.FromResult(new PageData<BlogPost>(pageIndex, pageSize, total, data));
        }

        Task<PageData<BlogPost>> IAppService.GetPostByAny(int pageIndex, int pageSize, string? keyword)
        {
            var posts = GetPostByAnyCore(_blogPosts, keyword);

            var total = posts.Count();
            var data = BlogPostToListCore(posts, pageIndex, pageSize);

            return Task.FromResult(new PageData<BlogPost>(pageIndex, pageSize, total, data));
        }

        Task<PageData<BlogPost>> IAppService.GetPostByCatSlug(int pageIndex, int pageSize, string? categorySlug)
        {
            //var json = _blogPosts.Where(p => p.Categories.Contains("分享")).Select(x => x.Title);
            //Console.WriteLine(string.Join(',', json));

            var posts = GetPostByCatSlugCore(_blogPosts, categorySlug);

            var total = posts.Count();
            var data = BlogPostToListCore(posts, pageIndex, pageSize);

            return Task.FromResult(new PageData<BlogPost>(pageIndex, pageSize, total, data));
        }
    }
}
