using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Categories;
using X.PagedList;

namespace PersonalblogServices.FPost
{
    public interface IFPostService
    {
        Task<IPagedList<Post>> GetFeaturedPostsAsync(QueryParameters param);
        FeaturedPost GetFeatures(int id);
        Task<List<FeaturedPost>> GetListAsync();
        int Delete(FeaturedPost featured);
        //排序
        Task<bool> UpdateSortOrderAsync(int featuredPostId, int newSortOrder);
        Task<(List<Post>, PaginationMetadata)> GetAllPostsAsync(QueryParameters param);
    }
}
