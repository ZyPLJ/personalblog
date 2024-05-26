using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
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
    public class FPostService : IFPostService
    {
        private readonly MyDbContext _myDbContext;
        public FPostService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public int Delete(FeaturedPost featured)
        {
            _myDbContext.Remove(featured);
            return _myDbContext.SaveChanges();
        }

        public async Task<bool> UpdateSortOrderAsync(int featuredPostId, int newSortOrder)
        {
            var fPost = await _myDbContext.featuredPosts.FindAsync(featuredPostId);
            if (fPost != null)
            {
                fPost.SortOrder = newSortOrder;
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<(List<Post>, PaginationMetadata)> GetAllPostsAsync(QueryParameters param)
        {
            var posts = await _myDbContext.posts
                .OrderByDescending(o => o.CreationTime)
                .Include(p => p.Categories)
                .Include(p => p.Comments)
                .Include(p => p.Tags)
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();
            var pagination = new PaginationMetadata()
            {
                PageNumber = param.Page,
                PageSize = param.PageSize,
                TotalItemCount = posts.Count
            };
            return (posts, pagination);
        }

        /// <summary>
        /// 这个查询语句会先加载 featuredPosts 表，然后使用 Include 方法加载文章表 Post，并使用 ThenInclude 方法一次性加载文章的分类和评论。
        ///注意，我们使用了两个 Include 方法来加载不同的关联表，并使用 ThenInclude 方法来指定要加载的关联表的属性。这样可以避免 EF Core 生成多个 SQL 查询，从而提高查询效率。
        ///在查询结果中，每个文章对象都包含了它的分类和评论。
        /// </summary>
        /// <returns></returns>
        public async Task<IPagedList<Post>> GetFeaturedPostsAsync(QueryParameters param)
        {
            // return _myDbContext.featuredPosts.Include(a => a.Post.Categories).Select(a => a.Post).ToList();
            var posts =await _myDbContext.featuredPosts
                .OrderBy(o => o.SortOrder)
                .Include(fp => fp.Post)
                .ThenInclude(p => p.Categories)
                .Include(fp => fp.Post)
                .ThenInclude(p => p.Comments)
                .Include(fp => fp.Post.Tags)
                .Select(fp => fp.Post)
                .ToPagedListAsync(param.Page, param.PageSize);
            return posts;
        }

        public FeaturedPost GetFeatures(int id)
        {
            return _myDbContext.featuredPosts.Where(a => a.Id == id)
                .Include(a => a.Post).First();
        }

        public async Task<List<FeaturedPost>> GetListAsync()
        {
            return await _myDbContext.featuredPosts.Include(a => a.Post.Categories).OrderBy(o => o.SortOrder).ToListAsync();
        }
    }
}
