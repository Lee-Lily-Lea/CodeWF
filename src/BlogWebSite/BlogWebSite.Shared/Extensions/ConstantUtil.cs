using BlogWebSite.Shared.Models;

namespace BlogWebSite.Shared.Extensions
{
    public static class ConstantUtil
    {
        public static string GetBbsPostUrl(BlogPost post) => $"./bbs/post/{post.Date?.Year}/{post.Date?.Month}/{post.Slug}";
        public static string GetBbsPostTagQueryUrl(string tag) => $"./bbs/bytag?hltag={Uri.EscapeDataString(tag)}";

        public static string GetPostGithubPath(string? githubRepository, BlogPost? post) =>
            $"{githubRepository}/blob/main/{post?.Date?.Year:D4}/{post?.Date?.Month:D2}/{post?.Slug}.md";
    }
}
