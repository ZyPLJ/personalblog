using Microsoft.EntityFrameworkCore;
using Personalblog.Migrate;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Blog;
using Personalblog.Utils;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using GuidUtils = Personalblog.Utils.GuidUtils;

namespace Personalblog.Services
{
    public class BlogService
    {
        private readonly MyDbContext _myDbContext;
        private readonly IWebHostEnvironment _environment;
        public BlogService(MyDbContext myDbContext, IWebHostEnvironment environment)
        {
            _myDbContext = myDbContext;
            _environment = environment;
        }
        /// <summary>
        /// 获取博客信息概况
        /// </summary>
        /// <returns></returns>
        public BlogOverview Overview()
        {
            return new BlogOverview()
            {
                PostsCount = _myDbContext.posts.Count(),
                CategoriesCount = _myDbContext.categories.Count(),
                PhotosCount = _myDbContext.photos.Count(),
                FeaturedPhotosCount = _myDbContext.featuredPhotos.Count(),
                FeaturedCategoriesCount = _myDbContext.featuredCategories.Count(),
                FeaturedPostsCount= _myDbContext.featuredPosts.Count(),
            };
        }
        public FeaturedPost AddFeaturedPost(Post post)
        {
            var item = _myDbContext.featuredPosts.Where(a => a.PostId == post.Id).FirstOrDefault();
            if (item != null) return item;
            item = new FeaturedPost { PostId = post.Id };
            _myDbContext.featuredPosts.Add(item);
            _myDbContext.SaveChanges();
            FeaturedPost fp = new FeaturedPost()
            {
                Id = item.Id,
                PostId = item.PostId,
                Post = new Post
                {
                    Id = item.Post.Id,
                    CategoryId = item.Post.CategoryId,
                    CreationTime = item.Post.CreationTime,
                    LastUpdateTime = item.Post.LastUpdateTime,
                    Path = item.Post.Path,
                    Summary = item.Post.Summary,
                    Title = item.Post.Title
                }
            };
            return fp;
        }
        public int DeleteFeaturedPost(Post post)
        {
            var item = _myDbContext.featuredPosts.Where(a => a.PostId == post.Id).First();
            _myDbContext.Remove(item);
            return item == null ? 0 : _myDbContext.SaveChanges();
        }
        /// <summary>
        /// 设置置顶博客
        /// </summary>
        /// <param name="post"></param>
        /// <returns>返回 <see cref="TopPost"/> 对象和删除原有置顶博客的行数</returns>
        public (TopPost, int) SetTopPost(Post post)
        {
            var data = _myDbContext.topPosts;
            _myDbContext.RemoveRange(data);
            var rows = _myDbContext.SaveChanges();
            var item = new TopPost { PostId = post.Id };
            _myDbContext.topPosts.Add(item);
            _myDbContext.SaveChanges();
            return (item, rows);
        }
        public Post? GetTopOnePost()
        {
            return _myDbContext.topPosts.Include(a => a.Post.Categories).FirstOrDefault()?.Post;
        }
        /// <summary>
        /// 上传md文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Post> Upload(int CategoryId, IFormFile file)
        {
            var tempFile = Path.GetTempFileName();
            await using (var fs = new FileStream(tempFile, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            var extractPath = Path.Combine(Path.GetTempPath(), "StarBlog", Guid.NewGuid().ToString());
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ZipFile.ExtractToDirectory(tempFile, extractPath, Encoding.GetEncoding("GBK"));

            var dir = new DirectoryInfo(extractPath);
            var files = dir.GetFiles("*.md");
            var mdFIle = files.First();
            using var reader = mdFIle.OpenText();
            var content = await reader.ReadToEndAsync();


            var post = new Post
            {
                Id = GuidUtils.GuidTo16String(),
                Title = mdFIle.Name.Replace(".md", ""),
                Summary = "",
                Content = content,
                Path = "",
                CreationTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                CategoryId = CategoryId,
                ViewCount = 0
            };
            // 处理文章正文内容
            var assetsPath = Path.Combine(_environment.WebRootPath, "media", "blog");
            // 导入文章的时候一并导入文章里的图片，并对图片相对路径做替换操作
            var processor = new PostProcessor(extractPath, assetsPath, post);

            post.Content = processor.MarkdownParse();
            if (string.IsNullOrEmpty(post.Summary))
            {
                post.Summary = processor.GetSummary(200);
            }
            // 存入数据库
            await _myDbContext.posts.AddAsync(post);
            await _myDbContext.SaveChangesAsync();
            return post;
        }
    }
}
