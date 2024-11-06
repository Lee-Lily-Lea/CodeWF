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
    public string? HlTag { get; set; }

    [Parameter]
    public EventCallback<string> TagChanged { get; set; }

    Task SearchByTag(string tag)
    {
        if (TagChanged.HasDelegate)
        {
            return TagChanged.InvokeAsync(tag);
        }

        return Task.CompletedTask;
    }
}
