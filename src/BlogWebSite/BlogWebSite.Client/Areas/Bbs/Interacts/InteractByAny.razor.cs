using BlogWebSite.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlogWebSite.Client.Areas.Bbs.Interacts
{
    public partial class InteractByAny : IInteracts
    {
        public const string Route = "/bbs";

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

            await CheckKeywordQuery();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            await CheckKeywordQuery();
        }

        [SupplyParameterFromQuery]
        public string? Keyword { get; set; }
        string? keyword = Guid.NewGuid().ToString();
        string GetSearchTooltip()
        {
            if (string.IsNullOrEmpty(Keyword) is false)
            {
                return $"关键词 '{Keyword}' 的搜索结果";
            }
            return string.Empty;
        }

        async Task CheckKeywordQuery()
        {
            if (keyword != Keyword)
            {
                keyword = Keyword;
                await OnPageIndexChanged(1);
            }
        }

        public async Task LoadData()
        {
            var blogPosts = await IAppService.GetPostByAny(pageIndex, pageSize, keyword);

            total = blogPosts.Total;
            SetPageLenght();
            blogs = blogPosts.Data;
        }

        [Obsolete("移动端无限加载的逻辑")]
        public async Task AddData()
        {
            var blogPosts = await IAppService.GetPostByAny(pageIndex + 1, pageSize, keyword);

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
