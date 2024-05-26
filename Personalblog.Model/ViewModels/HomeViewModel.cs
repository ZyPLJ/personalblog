using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personalblog.Model.ViewModels.Categories;
using Personalblog.Model.ViewModels.Home;
using X.PagedList;

namespace Personalblog.Model.ViewModels
{
    public class HomeViewModel
    {
        /// <summary>
        /// 推荐图片
        /// </summary>
        public List<Photo>? FeaturedPhotos { get; set; }
        /// <summary>
        /// 推荐分类 
        /// </summary>
        public List<FeaturedCategory>? FeaturedCategories { get; set; }
        /// <summary>
        /// 置顶博客
        /// </summary>
        public List<Post>? TopPost { get; set; }
        /// <summary>
        /// 友情链接
        /// </summary>
        public List<Link> Links { get; set; } = new();
        /// <summary>
        /// 推荐文章
        /// </summary>
        public IPagedList<Post>? FeaturedPosts { get; set; }
        /// <summary>
        /// 网站通知
        /// </summary>
        public List<Notice>? Notices { get; set; }
        /// <summary>
        /// 最新 最旧
        /// </summary>
        // public List<Post>? FirstLastPost { get; set; }
        /// <summary>
        /// 阅读最多
        /// </summary>
        public Post? MaxPost { get; set; }
        /// <summary>
        /// 阅读最多 评论最多 最新 最旧
        /// </summary>
        public HomePost HomePost { get; set; }
        public (List<Post>, PaginationMetadata) AllPosts { get; set; }
    }
}
