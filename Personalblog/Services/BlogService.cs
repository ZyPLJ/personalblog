using Microsoft.EntityFrameworkCore;
using Personalblog.Migrate;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Blog;
using Personalblog.Utils;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using Hangfire;
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
        public async Task<(TopPost, int)> SetTopPostAsync(Post post)
        {
            var data = _myDbContext.topPosts;
            if (data.Any(t => t.PostId == post.Id))
            {
                return (null, 0);
            }
            else
            {
                int rows = 0;
                if (data.Count() >= 3)
                {
                    var firstPost = await data.FirstAsync();
                    _myDbContext.topPosts.Remove(firstPost);
                    rows++;
                }
                var item = new TopPost { PostId = post.Id };
                _myDbContext.topPosts.Add(item);
                await _myDbContext.SaveChangesAsync();
                return (item, rows);
            }
        }
        public async Task<List<Post>?> GetTopOnePostAsync()
        {
            var topPost =await _myDbContext.topPosts.Include(a => a.Post.Categories).ToListAsync();
            return topPost.Select(tp => tp.Post).ToList();
        }
        /// <summary>
        /// 上传md文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Post> Upload(int CategoryId, List<string> tags,IFormFile file,DateTime? publishTime)
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
                ViewCount = 0,
            };
            //将文章实例添加到数据库上下文中
            await _myDbContext.posts.AddAsync(post);
            foreach (var tagName in tags)
            {
                var tag = await _myDbContext.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _myDbContext.Tags.Add(tag);
                }

                var postTag = new PostTag
                {
                    Post = post,
                    Tag = tag
                };

                _myDbContext.PostTags.Add(postTag);
            }
            // 处理文章正文内容
            var assetsPath = Path.Combine(_environment.WebRootPath, "media", "blog");
            // 导入文章的时候一并导入文章里的图片，并对图片相对路径做替换操作
            var processor = new PostProcessor(extractPath, assetsPath, post);

            post.Content = processor.MarkdownParse();
            if (string.IsNullOrEmpty(post.Summary))
            {
                post.Summary = processor.GetSummary(200);
            }
            
            if (publishTime.HasValue && publishTime.Value > DateTime.Now)
            {
                // 计算延迟时间
                var delay = publishTime.Value - DateTime.Now;
                post.Status = 0;//未发布
                // 使用Hangfire调度后台任务
                BackgroundJob.Schedule( () => UpdateStatus(post.Id) ,delay);
            }
            else
            {
                post.Status = 1; //发布
            }
            // 存入数据库
            await _myDbContext.SaveChangesAsync();
            return post;
        }

        public void UpdateStatus(string id)
        {
           var post = _myDbContext.posts.FirstOrDefault(p => p.Id == id);
           if (post != null)
           {
               post.Status = 1;
               _myDbContext.SaveChanges();
           }
        }
    }
}
