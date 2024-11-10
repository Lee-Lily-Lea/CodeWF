using BlogWebSite.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace BlogWebSite.Client.Areas.Bbs.Interacts
{
    public partial class InteractByCatSlug : IInteracts
    {
        public const string Route = "/bbs/byCatSlug";

        #region models
        int pageIndex;

        public async Task OnPageIndexChanged(int val)
        {
            pageIndex = val;
            await LoadData();
        }

        readonly int pageSize = 10;

        int total = -1;

        List<BlogPost> blogs = [];

        int pageLength;

        void SetPageLenght()
        {
            var length = total / pageSize;
            if (total % pageSize is not 0)
            {
                length += 1;
            }

            pageLength = length;
        }
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await CheckCatSlugQuery();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            await CheckCatSlugQuery();
        }

        [SupplyParameterFromQuery]
        public string? CatSlug { get; set; }
        string? catSlug = Guid.NewGuid().ToString();
        string GetSearchTooltip()
        {
            if (string.IsNullOrEmpty(CatSlug) is false)
            {
                return $"分类 '{CatSlug}' 的搜索结果";
            }
            return string.Empty;
        }
        async Task CheckCatSlugQuery()
        {
            if (catSlug != CatSlug)
            {
                catSlug = CatSlug;
                await OnPageIndexChanged(1);
            }
        }

        public async Task LoadData()
        {
            var blogPosts = await IAppService.GetPostByCatSlug(pageIndex, pageSize, catSlug);

            total = blogPosts.Total;
            SetPageLenght();
            blogs = blogPosts.Data;
        }

        [Obsolete("移动端无限加载的逻辑")]
        public async Task AddData()
        {
            var blogPosts = await IAppService.GetPostByCatSlug(pageIndex + 1, pageSize, catSlug);

            total = blogPosts.Total;
            blogs.AddRange(blogPosts.Data);
        }

        //private async Task OnLoad(InfiniteScrollLoadEventArgs args)
        //{
        //    var append = await IAppService.GetPostByCategory(_blogPosts.PageIndex + 1, _blogPosts.PageSize, Category, null);

        //    args.Status = append.Data.Count == 0 ? InfiniteScrollLoadStatus.Empty : InfiniteScrollLoadStatus.Ok;

        //    var cur = _blogPosts.Data;
        //    cur.AddRange(append.Data);

        //    _blogPosts = new(_blogPosts.PageIndex + 1, _blogPosts.PageSize, append.Total, cur);
        //}
    }
}
