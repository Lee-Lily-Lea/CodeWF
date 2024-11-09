using BlogWebSite.Shared;
using BlogWebSite.Shared.Models;
using YamlDotNet.Core.Tokens;

namespace BlogWebSite.Services
{
    public partial class AppService
    {
        #region BaseQuery

        private IEnumerable<CategoryItem> GetAllCategoryItemsCore(IEnumerable<CategoryItem>? source, string? keyword)
        {
            if (source is null)
            {
                return [];
            }

            if (string.IsNullOrEmpty(keyword))
            {
                return source;
            }

            var cats = source.Where(c =>
                c.Name?.Contains(keyword) == true || c.Slug?.Contains(keyword) == true
            );

            return cats;
        }

        #endregion



        Task<List<CategoryItem>> IAppService.GetAllCategoryItemsAsync()
        {
            var cats = GetAllCategoryItemsCore(_categoryItems, null);

            return Task.FromResult(cats.OrderBy(x => x.Sort).ToList());
        }
    }
}
