﻿using CodeWF.Options;
using CodeWF.Tools.Extensions;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CodeWF.Services;

public class AppService
{
    private readonly SiteOption _siteInfo;
    private List<DocItem>? _docItems;
    private List<CategotyItem>? _categotyItems;
    private List<BlogPost>? _blogPosts;
    private List<FriendLinkItem>? _friendLinkItems;
    private Dictionary<string, string>? _webSiteCountInfos;

    public AppService(IOptions<SiteOption> siteOption)
    {
        _siteInfo = siteOption.Value;
    }

    public async Task<List<DocItem>?> GetAllDocItemsAsync()
    {
        if (_docItems?.Any() == true)
        {
            return _docItems;
        }

        var filePath = Path.Combine(_siteInfo.LocalAssetsDir, "site", "doc", "doc.json");
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
            contentPath = string.IsNullOrWhiteSpace(parentDir)
                ? Path.Combine(_siteInfo.LocalAssetsDir, "site", "doc", $"{item.Slug}.md")
                : Path.Combine(_siteInfo.LocalAssetsDir, "site", "doc", parentDir, $"{item.Slug}.md");

            if (File.Exists(contentPath))
            {
                item.Content = await File.ReadAllTextAsync(contentPath);
            }
        }

        var items = await GetAllDocItemsAsync();
        if (items?.Any() != true)
        {
            return default;
        }

        foreach (var item in items)
        {
            if (item.Slug == slug)
            {
                await ReadContentAsync(item);
                return item;
            }

            if (item.Children?.Any() != true) continue;

            foreach (var itemChild in item.Children.Where(itemChild => itemChild.Slug == slug))
            {
                await ReadContentAsync(itemChild, item.Slug);
                return itemChild;
            }
        }

        var first = items.FirstOrDefault();
        await ReadContentAsync(first);

        return first;
    }

    public async Task<List<CategotyItem>?> GetAllCategoryItemsAsync()
    {
        if (_categotyItems?.Any() == true)
        {
            return _categotyItems;
        }

        var filePath = Path.Combine(_siteInfo.LocalAssetsDir, "site", "category.json");
        if (!File.Exists(filePath))
        {
            return _categotyItems;
        }

        var fileContent = await File.ReadAllTextAsync(filePath);
        fileContent.FromJson(out _categotyItems, out var msg);
        return _categotyItems;
    }


    public async Task<List<BlogPost>?> GetAllBlogPostsAsync()
    {
        if (_blogPosts?.Any() == true)
        {
            return _blogPosts;
        }

        _blogPosts = new();
        var endYear = DateTime.Now.Year;

        for (var start = _siteInfo.StartYear; start <= endYear; start++)
        {
            var postDir = Path.Combine(_siteInfo.LocalAssetsDir, start.ToString());
            var postFiles = Directory.GetFiles(postDir, "*.md", SearchOption.AllDirectories);
            foreach (var postFile in postFiles)
            {
                var blogPost = await ReadBlogPostAsync(postFile);
                _blogPosts.Add(blogPost);
            }
        }

        return _blogPosts;
    }

    public async Task<PageData<BlogPost>?> GetPostByCategory(int pageIndex, int pageSize, string categorySlug,
        string? key)
    {
        var cat = (await GetAllCategoryItemsAsync())?.FirstOrDefault(cat => cat.Slug == categorySlug);
        if (cat == null)
        {
            return default;
        }

        IEnumerable<BlogPost> posts;
        if (!string.IsNullOrWhiteSpace(key))
        {
            posts = (await GetAllBlogPostsAsync())
                ?.Where(p => p.Title?.Contains(key) == true
                             || p.Description?.Contains(key) == true
                             || p.Slug?.Contains(key) == true
                             || p.Author?.Contains(key) == true
                             || p.LastModifyUser?.Contains(key) == true
                             || p.Content?.Contains(key) == true);
        }
        else
        {
            posts = (await GetAllBlogPostsAsync())
                ?.Where(post => post.Categories?.Contains(cat.Name) == true);
        }

        var total = posts.Count();

        var postDatas = posts
            .OrderByDescending(post => post.Lastmod)
            .ThenByDescending(post => post.Date)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return new PageData<BlogPost>(pageIndex, pageSize, total, postDatas);
    }

    public async Task<List<BlogPost>?> GetBannerPostAsync()
    {
        var posts = (await GetAllBlogPostsAsync())
            ?.Where(post => post.Banner)
            .OrderByDescending(post => post.Date)
            .ToList();
        return posts;
    }

    public async Task<Dictionary<string, string>> GetWebSiteCountAsync()
    {
        if (_webSiteCountInfos != null)
        {
            return _webSiteCountInfos;
        }

        _webSiteCountInfos = new();
        await GetAllBlogPostsAsync();
        await GetAllCategoryItemsAsync();
        var total = _blogPosts?.Count;
        var original = _blogPosts?.Count(post => string.IsNullOrWhiteSpace(post.Author));
        var originalPercentage = (double)original / total * 100;
        _webSiteCountInfos["网站创建"] = $"{DateTime.Now.Year - _siteInfo.StartYear}年";
        _webSiteCountInfos["文章分类"] = $"{_categotyItems.Count}个";
        _webSiteCountInfos["文章总计"] = $"{total}篇";
        _webSiteCountInfos["文章原创"] = $"{original}篇({originalPercentage:F2}%)";
        return _webSiteCountInfos;
    }

    public async Task<BlogPost?> GetPostBySlug(string slug)
    {
        var post = (await GetAllBlogPostsAsync())?.FirstOrDefault(cat => cat.Slug == slug);
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

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

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

        return blogPost;
    }

    public async Task<List<FriendLinkItem>?> GetAllFriendLinkItemsAsync()
    {
        if (_friendLinkItems?.Any() == true)
        {
            return _friendLinkItems;
        }

        var filePath = Path.Combine(_siteInfo.LocalAssetsDir, "site", "FriendLink.json");
        if (!File.Exists(filePath))
        {
            return _friendLinkItems;
        }

        var fileContent = await File.ReadAllTextAsync(filePath);
        fileContent.FromJson(out _friendLinkItems, out var msg);
        return _friendLinkItems;
    }
}