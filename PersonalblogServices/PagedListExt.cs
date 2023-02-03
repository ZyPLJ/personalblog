using System.Text.Json;
using Personalblog.Model.ViewModels.Categories;
using X.PagedList;

namespace PersonalblogServices
{
    public static class PagedListExt
    {
        public static PaginationMetadata ToPaginationMetadata(this IPagedList page)
        {
            return new PaginationMetadata
            {
                PageCount = page.PageCount,
                TotalItemCount = page.TotalItemCount,
                PageNumber = page.PageNumber,
                PageSize = page.PageSize,
                HasNextPage = page.HasNextPage,
                HasPreviousPage = page.HasPreviousPage,
                IsFirstPage = page.IsFirstPage,
                IsLastPage = page.IsLastPage,
                FirstItemOnPage = page.FirstItemOnPage,
                LastItemOnPage = page.LastItemOnPage
            };
        }

        public static string ToPaginationMetadataJson(this IPagedList page)
        {
            return JsonSerializer.Serialize(ToPaginationMetadata(page));
        }
    }
}