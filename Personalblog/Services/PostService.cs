using Markdig.Renderers.Normalize;
using Markdig.Syntax.Inlines;
using Markdig.Syntax;
using Markdig;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using System.Linq;
using System.Linq.Expressions;
using X.PagedList;
using System.Net;
using Personalblog.Model.Extensions.Markdown;
using Personalblog.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Personalblog.Services
{
    public class PostService
    {
        private readonly MyDbContext _myDbContext;
        private readonly ConfigService _conf;
        private readonly IWebHostEnvironment _environment;
        private readonly CommonService _commonService;

        public PostService(MyDbContext myDbContext,
            ConfigService conf, 
            IWebHostEnvironment environment,
            CommonService commonService)
        {
            _myDbContext = myDbContext;
            _conf = conf;
            _environment = environment;
            _commonService = commonService;
        }
        public string Host => _conf["host"];
        //public string Host => "https://localhost:44310/";
        public IPagedList<Post> GetPagedList(PostQueryParameters param)
        {
            var querySet = _myDbContext.posts
                .Include(a => a.Categories)
                .Include(a => a.Tags)
                .Select(a=>new Post
            {
                Id = a.Id,
                Categories = a.Categories,
                CategoryId = a.CategoryId,
                CreationTime = a.CreationTime,
                LastUpdateTime = a.LastUpdateTime,
                Path = a.Path,
                Summary = a.Summary,
                Title = a.Title,
                ViewCount = a.ViewCount,
                Tags = a.Tags,
                Status = a.Status
            });
            //分类过滤
            if(param.CategoryId != 0)
            {
                querySet = querySet.Where(a => a.CategoryId == param.CategoryId);
            }
            // 关键词过滤
            if (!string.IsNullOrEmpty(param.Search))
            {
                querySet = querySet.Where(a => a.Title.Contains(param.Search));
            }
            //标签过滤
            if (param.TagId != 0)
            {
                querySet = querySet.Where(a => a.Tags.Any(t => t.Id == param.TagId));
            }
            // 排序
            if (!string.IsNullOrEmpty(param.SortBy))
            {
                // 是否升序
                var isAscending = !param.SortBy.StartsWith("-");
                var orderByProperty = param.SortBy.Trim('-');
                if (isAscending) querySet = querySet.OrderBy(a => a.LastUpdateTime);
                else querySet = querySet.OrderByDescending(a => a.LastUpdateTime);
            }

            return querySet.ToPagedList(param.Page, param.PageSize);
        }
        public Post? GetById(string id)
        {
            var post = _myDbContext.posts.Where(a => a.Id == id)
                .Include(a => a.Categories)
                .Include(a => a.Tags)
                .First();
            if (post != null) post.Content = MdImageLinkConvert(post, true);
            return post;
        }
        public int Delete(Post p)
        {
            _myDbContext.Remove(p);
            return _myDbContext.SaveChanges();
        }
        /// <summary>
        /// Markdown中的图片链接转换
        /// <para>支持添加或去除Markdown中的图片URL前缀</para>
        /// </summary>
        /// <param name="post"></param>
        /// <param name="isAddPrefix">是否添加本站的完整URL前缀</param>
        /// <returns></returns>
        private string MdImageLinkConvert(Post post, bool isAddPrefix = true)
        {
            if (post.Content == null) return string.Empty;
            var document = Markdown.Parse(post.Content);

            foreach (var node in document.AsEnumerable())
            {
                if (node is not ParagraphBlock { Inline: { } } paragraphBlock) continue;
                foreach (var inline in paragraphBlock.Inline)
                {
                    if (inline is not LinkInline { IsImage: true } linkInline) continue;

                    var imgUrl = linkInline.Url;
                    if (imgUrl == null) continue;

                    // 已有Host前缀，跳过
                    if (isAddPrefix && imgUrl.StartsWith(Host)) continue;

                    // 设置完整链接
                    if (isAddPrefix)
                    {
                        if (imgUrl.StartsWith("http")) continue;
                        linkInline.Url = $"{Host}{imgUrl}";
                    }
                    // 设置成相对链接
                    else
                    {
                        linkInline.Url = $"/media/blog/{post.Id}/{Path.GetFileName(imgUrl)}";
                    }
                }
            }

            using var writer = new StringWriter();
            var render = new NormalizeRenderer(writer);
            render.Render(document);
            return writer.ToString();
        }
        public async Task<Post> InsertOrUpdateAsync(Post post)
        {
            await using var transaction = await _myDbContext.Database.BeginTransactionAsync();

            try
            {
                var tags = post.Tags.ToList();
                // 移除原有的标签
                post.Tags.Clear();
                // 是新文章的话，先保存到数据库
                if (await _myDbContext.posts.FindAsync(post.Id) == null)
                {
                    post.ViewCount = 0;
                    await _myDbContext.posts.AddAsync(post);
                }
                else
                {
                    var existingTags = await _myDbContext.PostTags
                        .Where(pt => pt.PostId == post.Id)
                        .ToListAsync();
                    _myDbContext.PostTags.RemoveRange(existingTags);
                }
                //修改
                foreach (var tagName in tags)
                {
                    var postTag = new PostTag
                    {
                        PostId = post.Id,
                        TagId = tagName.Id,
                    };

                    _myDbContext.PostTags.Add(postTag);
                }

                // 保存更改到数据库
                await _myDbContext.SaveChangesAsync();

                // 检查文章中的外部图片，下载并进行替换
                post.Content = await MdExternalUrlDownloadAsync(post);

                // 修改文章时，将markdown中的图片地址替换成相对路径再保存
                post.Content = MdImageLinkConvert(post, false);

                // 更新文章
                _myDbContext.posts.Attach(post);
                _myDbContext.Entry(post).State = EntityState.Modified;
                await _myDbContext.SaveChangesAsync();

                // 提交事务
                await transaction.CommitAsync();

                return post;
            }
            catch (Exception ex)
            {
                // 回滚事务
                await transaction.RollbackAsync();
                throw ex;
            }
        }
        /// <summary>
        /// 初始化博客文章的资源目录
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private string InitPostMediaDir(Post post)
        {
            var blogMediaDir = Path.Combine(_environment.WebRootPath, "media", "blog");
            var postMediaDir = Path.Combine(_environment.WebRootPath, "media", "blog", post.Id);
            if (!Directory.Exists(blogMediaDir)) Directory.CreateDirectory(blogMediaDir);
            if (!Directory.Exists(postMediaDir)) Directory.CreateDirectory(postMediaDir);

            return postMediaDir;
        }
        /// <summary>
        /// Markdown中外部图片下载
        /// <para>如果Markdown中包含外部图片URL，则下载到本地且进行URL替换</para>
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private async Task<string> MdExternalUrlDownloadAsync(Post post)
        {
            if (post.Content == null) return string.Empty;
            // 得先初始化目录
            InitPostMediaDir(post);

            var document = Markdown.Parse(post.Content);
            foreach (var node in document.AsEnumerable())
            {
                if (node is not ParagraphBlock { Inline: { } } paragraphBlock) continue;
                foreach (var inline in paragraphBlock.Inline)
                {
                    if (inline is not LinkInline { IsImage: true } linkInline) continue;

                    var imgUrl = linkInline.Url;
                    // 跳过空链接
                    if (imgUrl == null) continue;
                    // 跳过本站地址的图片
                    if (imgUrl.StartsWith(Host)) continue;

                    // 下载图片
                    //_logger.LogDebug("文章：{Title}，下载图片：{Url}", post.Title, imgUrl);
                    var savePath = Path.Combine(_environment.WebRootPath, "media", "blog", post.Id!);
                    var fileName = await _commonService.DownloadFileAsync(imgUrl, savePath);
                    linkInline.Url = fileName;
                }
            }
            await using var writer = new StringWriter();
            var render = new NormalizeRenderer(writer);
            render.Render(document);
            return writer.ToString();
        }
        /// <summary>
        /// 指定文章上传图片
        /// </summary>
        /// <param name="post"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UploadImage(Post post, IFormFile file)
        {
            InitPostMediaDir(post);

            var filename = file.FileName;
            var fileRelativePath = Path.Combine("media", "blog", post.Id!, filename);
            var savePath = Path.Combine(_environment.WebRootPath, fileRelativePath);
            // 生成临时文件名
            var tempFilename = $"{GuidUtils.GuidTo16String()}{Path.GetExtension(filename)}";
            var tempPath = Path.Combine(_environment.WebRootPath, "temp", tempFilename);

            using (var fs = new FileStream(tempPath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            // 压缩图片
            using (var image = Image.Load(tempPath))
            {
                var encoder = new JpegEncoder()
                {
                    Quality = 70 // 设置压缩质量
                };
                image.Save(savePath,encoder);
            }

            // 删除临时文件
            File.Delete(tempPath);

            return Path.Combine(Host, fileRelativePath);
        }
        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="imagePath">原路径</param>
        /// <param name="outputPath">新路径</param>
        /// <param name="quality">压缩质量 1-100 数值越低压缩越多</param>
        public void CompressImage(string imagePath, string outputPath, int quality)
        {
            using var image = Image.Load(imagePath);
            var encoder = new JpegEncoder()
            {
                Quality = quality // 设置压缩质量
            };
            image.Save(outputPath, encoder);
        }
        public static string GetContentHtml(Post post) {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseBootstrap5()
                .Build();
            return Markdown.ToHtml(post.Content ?? "", pipeline);
        }
        // public static void CompressImage(string inputImagePath, string outputDirectory, int quality)
        // {
        //     // 加载原始图片
        //     using var image = SixLabors.ImageSharp.Image.Load(inputImagePath);
        //
        //     // 设置压缩选项
        //     var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
        //     {
        //         Quality = quality
        //     };
        //
        //     // 生成输出图片路径
        //     string fileName = Path.GetFileName(inputImagePath);
        //     //string outputImagePath = Path.Combine(outputDirectory, fileName);
        //
        //     // 保存压缩后的图片
        //     using var outputStream = new FileStream(outputDirectory, FileMode.Create);
        //     image.Save(outputStream, encoder);
        // }
    }
}
