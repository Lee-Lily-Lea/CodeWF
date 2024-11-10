using BlogWebSite.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace BlogWebSite.Client.Areas.Bbs.Interacts.Componets;

public partial class InteractContent
{
    public const string Title = "博文";

    [Parameter]
    public int PageIndex { get; set; }

    [Parameter]
    public EventCallback<int> PageIndexChanged { get; set; }

    [Parameter]
    public int PageSize { get; set; }

    [Parameter]
    public int PageLength { get; set; }

    [Parameter]
    public List<BlogPost> BlogPosts { get; set; } = [];


    [Parameter]
    public bool NoContent { get; set; }

    [SupplyParameterFromQuery]
    public string? HlTag { get; set; }

    [SupplyParameterFromQuery]
    public string? Author { get; set; }

    [SupplyParameterFromQuery]
    public string? CatSlug { get; set; }

    [SupplyParameterFromQuery]
    public string? Keyword { get; set; }
    
    string GetTagUrl(string? tag)
    {
        var url = GetQuery(InteractByTag.Route, nameof(HlTag), tag);

        return url;
    }

    string GetAuthorUrl(string? author)
    {
        var url = GetQuery(InteractByAuthor.Route, nameof(Author), author);

        return url;
    }

    string GetCatSlugUrl(string? cat)
    {
        var url = GetQuery(InteractByCatSlug.Route, nameof(CatSlug), cat);

        return url;
    }
    string GetKeywordUrl(string? keyword)
    {
        var url = GetQuery(InteractByAny.Route, nameof(Keyword), keyword);

        return url;
    }

    static string GetQuery(string baseUrl, string key, string? val)
    {
        if (val is null)
        {
            return baseUrl;
        }

        return baseUrl + '?' + key + '=' + Uri.EscapeDataString(val);
    }

    void SearchByTag(string? tag)
    {
        if (tag == HlTag)
        {
            tag = null;
        }
        var url = GetTagUrl(tag);
        NavigationManager.NavigateTo(url);
    }

    void SearchByAuthor(string? author)
    {
        author ??= BlogPost.DefaultAuthor;

        if (author == Author)
        {
            author = string.Empty;
        }

        var url = GetAuthorUrl(author);
        NavigationManager.NavigateTo(url);
    }

    void SearchByCatSlug(string? catSlug)
    {
        if (catSlug == CatSlug)
        {
            catSlug = null;
        }
        var url = GetCatSlugUrl(catSlug);
        NavigationManager.NavigateTo(url);
    }

    [Parameter]
    public string SearchTooltip { get; set; }
    bool searchOpen;
    void SearchByAny()
    {
        //if (keyword == Keyword)
        //{
        //    keyword = null;
        //}
        searchOpen = false;
        var url = GetKeywordUrl(keyword);
        NavigationManager.NavigateTo(url);
    }


    string GetTopLink() => GetLink(Title);
    string GetLink(string id) => NavigationManager.Uri.Split('#')[0] + '#' + id;

    string? keyword;
    protected override async void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (firstRender)
        {
            var titles = BlogPosts.Select(x => new
            {
                Type = "h2",
                Id = x.Slug,
                Title = x.Title,
            });

            await Js.InvokeVoidAsync("tocUp", 100, 800);
            await Js.InvokeVoidAsync("watchTOC", 100);

            keyword = Keyword;
        }
    }


    List<CategoryItem> cats = [];
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var cats = await IAppService.GetAllCategoryItems();
        this.cats = cats ?? [];
    }

    StringNumber curItem;


    static string GetBbsPostUrl(BlogPost post)
    {
        return PostPages.Index.GetUrl(post.Date?.Year ?? 0, post.Date?.Month ?? 0, post.Slug ?? string.Empty);
    }
}
