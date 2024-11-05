﻿using BlogWebSite.Services.Configure;
using BlogWebSite.Services.Extensions;
using BlogWebSite.Shared;
using BlogWebSite.Shared.Models;
using CodeWF.Tools.Extensions;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BlogWebSite.Services
{
    [Obsolete("需要替换掉")]
    public class AppService(SiteOption siteOption) : IAppService
    {
        private readonly SiteOption _siteOption = siteOption;
        private List<DocItem>? _docItems;
        private List<CategoryItem>? _categoryItems;
        private List<BlogPost>? _blogPosts;
        private List<FriendLinkItem>? _friendLinkItems;
        private Dictionary<string, string>? _webSiteCountInfos;
        private string? _donationContent;
        private string? _aboutContent;

        public async Task SeedAsync()
        {
            await GetAllCategoryItemsAsync();
            await GetAllBlogPostsAsync();
            await GetAllFriendLinkItemsAsync();
            await GetAllDocItemsAsync();
            await GetWebSiteCountAsync();
            await ReadAboutAsync();
            await ReadDonationAsync();
            Console.WriteLine("seed ok");
        }

        public async Task<List<DocItem>?> GetAllDocItemsAsync()
        {
            if (_docItems?.Any() == true)
            {
                return _docItems;
            }

            var filePath = Path.Combine(GetPath(), "site", "doc", "doc.json");
            if (!File.Exists(filePath))
            {
                return _docItems;
            }

            var fileContent = await File.ReadAllTextAsync(filePath);
            fileContent.FromJson(out _docItems, out var msg);
            return _docItems;
        }

        public async Task<DocItem?> GetDocItemAsync(string slug)
        {
            async Task ReadContentAsync(DocItem item, string? parentDir = default)
            {
                if (!string.IsNullOrWhiteSpace(item.Content))
                {
                    return;
                }

                var contentPath = string.Empty;
                contentPath = string.IsNullOrWhiteSpace(parentDir) ? Path.Combine(GetPath(), "site", "doc", $"{item.Slug}.md") : Path.Combine(GetPath(), "site", "doc", parentDir, $"{item.Slug}.md");

                if (File.Exists(contentPath))
                {
                    item.Content = await File.ReadAllTextAsync(contentPath);
                    item.HtmlContent = item.Content.ToHtml();
                }
            }

            if (_docItems?.Any() != true)
            {
                return default;
            }

            foreach (var item in _docItems)
            {
                if (item.Slug == slug)
                {
                    await ReadContentAsync(item);
                    return item;
                }

                if (item.Children?.Any() != true)
                    continue;

                foreach (var itemChild in item.Children.Where(itemChild => itemChild.Slug == slug))
                {
                    await ReadContentAsync(itemChild, item.Slug);
                    return itemChild;
                }
            }

            var first = _docItems.FirstOrDefault();
            await ReadContentAsync(first);

            return first;
        }

        public async Task<List<CategoryItem>?> GetAllCategoryItemsAsync()
        {
            if (_categoryItems?.Any() == true)
            {
                return _categoryItems;
            }

            var filePath = Path.Combine(GetPath(), "site", "category.json");

            var s = Directory.GetCurrentDirectory();
             if (!File.Exists(filePath))
            {
                return _categoryItems;
            }

            var fileContent = await File.ReadAllTextAsync(filePath);
            fileContent.FromJson(out _categoryItems, out var msg);
            return _categoryItems;
        }

        [Obsolete("使用的本地路径, 要替换")]
        private string GetPath()
        {
            var appCtxDir = AppContext.BaseDirectory; // exe执行路径;
            return Path.Combine(appCtxDir, _siteOption.LocalAssetsDir);
        }

        public async Task<List<BlogPost>?> GetAllBlogPostsAsync()
        {
            if (_blogPosts?.Any() == true)
            {
                return _blogPosts;
            }

            _blogPosts = new();
            var endYear = DateTime.Now.Year;

            for (var start = _siteOption.StartYear; start <= endYear; start++)
            {
                var postDir = Path.Combine(GetPath(), start.ToString());
                if (!Directory.Exists(postDir))
                {
                    continue;
                }

                var postFiles = Directory.GetFiles(postDir, "*.md", SearchOption.AllDirectories);
                foreach (var postFile in postFiles)
                {
                    var blogPost = await ReadBlogPostAsync(postFile);
                    _blogPosts.Add(blogPost);
                }
            }

            return _blogPosts;
        }

        public Task<PageData<BlogPost>> GetPostByCategory(int pageIndex, int pageSize, string categorySlug, string? key)
        {
            PageData<BlogPost> ret = new(pageIndex, pageSize, 0, []);

            var cat = _categoryItems?.FirstOrDefault(cat => cat.Slug == categorySlug);
            if (cat != null)
            {
                IEnumerable<BlogPost> posts;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    posts = _blogPosts?.Where(p => p.Title?.Contains(key) == true || p.Description?.Contains(key) == true || p.Slug?.Contains(key) == true || p.Author?.Contains(key) == true || p.LastModifyUser?.Contains(key) == true || p.Content?.Contains(key) == true);
                }
                else
                {
                    posts = _blogPosts?.Where(post => post.Categories?.Contains(cat.Name) == true);
                }

                var total = posts.Count();

                var postDatas = posts.OrderByDescending(post => post.Lastmod).ThenByDescending(post => post.Date).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                ret = new(pageIndex, pageSize, total, postDatas);
            }

            return Task.FromResult(ret);
        }

        public async Task<List<BlogPost>?> GetBannerPostAsync()
        {
            var posts = _blogPosts?.Where(post => post.Banner).OrderByDescending(post => post.Date).ToList();
            return posts;
        }

        public async Task<Dictionary<string, string>> GetWebSiteCountAsync()
        {
            if (_webSiteCountInfos != null)
            {
                return _webSiteCountInfos;
            }

            _webSiteCountInfos = new();
            var total = _blogPosts?.Count;
            var original = _blogPosts?.Count(post => string.IsNullOrWhiteSpace(post.Author));
            var originalPercentage = (double)original / total * 100;
            _webSiteCountInfos["网站创建"] = $"{DateTime.Now.Year - _siteOption.StartYear}年";
            _webSiteCountInfos["文章分类"] = $"{_categoryItems?.Count}个";
            _webSiteCountInfos["文章总计"] = $"{total}篇";
            _webSiteCountInfos["文章原创"] = $"{original}篇({originalPercentage:F2}%)";
            return _webSiteCountInfos;
        }

        public async Task<string?> ReadAboutAsync()
        {
            if (!string.IsNullOrWhiteSpace(_aboutContent))
            {
                return _aboutContent;
            }

            var filePath = Path.Combine(GetPath(), "site", "about.md");
            if (!File.Exists(filePath))
            {
                return "## 关于";
            }

            var content = await File.ReadAllTextAsync(filePath);
            _aboutContent = content.ToHtml();
            return _aboutContent;
        }

        public async Task<string?> ReadDonationAsync()
        {
            if (!string.IsNullOrWhiteSpace(_donationContent))
            {
                return _donationContent;
            }

            var filePath = Path.Combine(GetPath(), "site", "pays", "Donation.md");
            if (!File.Exists(filePath))
            {
                return "## 赞助";
            }

            var content = await File.ReadAllTextAsync(filePath);
            _donationContent = content.ToHtml();
            return _donationContent;
        }

        public async Task<BlogPost?> GetPostBySlug(string slug)
        {
            var post = _blogPosts?.FirstOrDefault(cat => cat.Slug == slug);
            return post;
        }

        public static async Task<BlogPost> ReadBlogPostAsync(string markdownFilePath)
        {
            var markdown = await File.ReadAllTextAsync(markdownFilePath);
            var endOfFrontMatter = markdown.IndexOf("---", 3, StringComparison.Ordinal);
            if (endOfFrontMatter == -1)
            {
                throw new InvalidOperationException("Invalid markdown format. No ending '---' found for Front Matter.");
            }

            var frontMatterText = markdown[..(endOfFrontMatter + 3)];
            var markdownContent = markdown[(endOfFrontMatter + 3)..].Trim();
            if (frontMatterText.StartsWith("---"))
            {
                frontMatterText = frontMatterText[3..].Trim();
            }

            if (frontMatterText.EndsWith("---"))
            {
                frontMatterText = frontMatterText[..^3].Trim();
            }

            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

            BlogPost blogPost;
            try
            {
                blogPost = deserializer.Deserialize<BlogPost>(frontMatterText);
            }
            catch (Exception ex)
            {
                string a = ex.Message;

                blogPost = new BlogPost();
            }

            blogPost.Content = markdownContent;
            blogPost.HtmlContent = markdownContent.ToHtml();

            return blogPost;
        }

        public async Task<List<FriendLinkItem>?> GetAllFriendLinkItemsAsync()
        {
            if (_friendLinkItems?.Any() == true)
            {
                return _friendLinkItems;
            }

            var filePath = Path.Combine(GetPath(), "site", "FriendLink.json");
            if (!File.Exists(filePath))
            {
                return _friendLinkItems;
            }

            var fileContent = await File.ReadAllTextAsync(filePath);
            fileContent.FromJson(out _friendLinkItems, out var msg);
            return _friendLinkItems;
        }
    }
}
