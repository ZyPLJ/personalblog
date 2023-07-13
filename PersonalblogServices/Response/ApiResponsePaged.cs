using Personalblog.Model.ViewModels.Categories;
using X.PagedList;

namespace PersonalblogServices.Response
{
    public class ApiResponsePaged<T> : ApiResponse<List<T>> where T : class
    {
        public ApiResponsePaged()
        {
        }

        public ApiResponsePaged(IPagedList<T> pagedList)
        {
            Data = pagedList.ToList();
            Pagination = pagedList.ToPaginationMetadata();
        }
        
        public ApiResponsePaged(List<T> data, PaginationMetadata pagination)
        {
            this.Data = data;
            this.Pagination = pagination;
            this.Pagination.PageCount = Convert.ToInt64(Math.Ceiling((double) this.Pagination.TotalItemCount / (double) this.Pagination.PageSize));
            this.Pagination.HasPreviousPage = this.Pagination.PageNumber > 1L;
            this.Pagination.HasNextPage = this.Pagination.PageNumber < this.Pagination.PageCount - 1L;
            this.Pagination.IsFirstPage = this.Pagination.PageNumber == 1L;
            this.Pagination.IsLastPage = this.Pagination.PageNumber == this.Pagination.PageCount;
            this.Pagination.FirstItemOnPage = 1L;
            this.Pagination.LastItemOnPage = this.Pagination.PageCount;
        }
        public PaginationMetadata? Pagination { get; set; }
    }
}
