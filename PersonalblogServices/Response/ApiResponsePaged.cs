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

        public PaginationMetadata? Pagination { get; set; }
    }
}
