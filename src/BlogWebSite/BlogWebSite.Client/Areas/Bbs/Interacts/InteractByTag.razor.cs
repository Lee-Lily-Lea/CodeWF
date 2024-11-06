using BlogWebSite.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlogWebSite.Client.Areas.Bbs.Interacts
{
    public partial class InteractByTag : IInteracts
    {
        string GetUrl(string? tag) => NavigationManager.GetRelativeUriWithQueryParameter(nameof(HlTag), tag);

        void OnTagChanged(string tag)
        {
            if (tag == HlTag)
            {
                tag = string.Empty;
            }
            var url = GetUrl(tag);
            NavigationManager.NavigateTo(url);
        }

        //bool lite;

        #region models
        int pageIndex;

        public async Task OnPageIndexChanged(int val)
        {
            pageIndex = val;
            await LoadData();
        }

        readonly int pageSize = 10;

        int total;

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await CheckTagQuery();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await CheckTagQuery();
        }

        [SupplyParameterFromQuery]
        public string? HlTag { get; set; }
        string? tag = string.Empty;

        async Task CheckTagQuery()
        {
            if (tag != HlTag)
            {
                tag = HlTag;
                await OnPageIndexChanged(1);
            }
        }

        public async Task LoadData()
        {
            var blogPosts = await IAppService.GetPostByTag(pageIndex, pageSize, tag);

            total = blogPosts.Total;
            SetPageLenght();
            blogs = blogPosts.Data;

            Console.WriteLine("-------load data---------");
            Console.WriteLine(nameof(pageIndex) + ":\t" + pageIndex);
            Console.WriteLine(nameof(pageSize) + ":\t" + pageSize);
            Console.WriteLine(nameof(total) + ":\t" + total);
            Console.WriteLine(nameof(pageLength) + ":\t" + pageLength);
            Console.WriteLine("-------load end---------\r\n");
        }

        [Obsolete("移动端无限加载的逻辑")]
        public async Task AddData()
        {
            var blogPosts = await IAppService.GetPostByTag(pageIndex + 1, pageSize, tag);

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
