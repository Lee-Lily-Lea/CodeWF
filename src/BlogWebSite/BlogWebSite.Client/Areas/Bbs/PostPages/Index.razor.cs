using BlogWebSite.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlogWebSite.Client.Areas.Bbs.PostPages
{
    public partial class Index
    {
        public const string Route = RouteBase + RouteYear + RouteMonth + RouteSlug;
        public const string RouteBase = "/bbs/post";
        public const string RouteYear = $"/{{{nameof(Year)}:int}}";
        public const string RouteMonth = $"/{{{nameof(Month)}:int}}";
        public const string RouteSlug = $"/{{{nameof(Slug)}}}";

        public static string GetUrl(int year, int month, string slug)
        {
            return string.Join(
                '/',
                RouteBase,
                Uri.EscapeDataString(year.ToString()),
                Uri.EscapeDataString(month.ToString()),
                Uri.EscapeDataString(slug)
            );
        }

        [Parameter]
        public int Year { get; set; }

        [Parameter]
        public int Month { get; set; }

        [Parameter]
        public string Slug { get; set; }

        private BlogPost? model;

        Guid mdId = Guid.NewGuid();
        string GetTopLink() => NavigationManager.Uri.Split('#')[0] + '#' + mdId;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            model = await IAppService.GetPostBySlug(Slug);
        }
    }
}
