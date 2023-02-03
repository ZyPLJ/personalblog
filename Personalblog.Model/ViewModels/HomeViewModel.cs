using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Post? TopPost { get; set; }
        /// <summary>
        /// 推荐文章
        /// </summary>
        public List<Post>? FeaturedPosts { get; set; }
    }
}
