using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Blog;
using Personalblog.Services;
using PersonalblogServices.Categorys;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    [ApiController]
    [Route("Api/[controller]")]
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;
        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// 博客信息概况
        /// </summary>
        /// <returns></returns>
        // [Authorize]
        [HttpGet("[action]")]
        public ApiResponse<BlogOverview> Overview()
        {
            return new ApiResponse<BlogOverview>(_blogService.Overview());
        }
        /// <summary>
        /// 获取置顶博客
        /// </summary>
        /// <returns></returns>
        [HttpGet("Top")]
        public async Task<ApiResponse<List<Post>>> GetTopOnePost()
        {
            return new ApiResponse<List<Post>> { Data =await _blogService.GetTopOnePostAsync() };
        }
        /// <summary>
        /// 上传博客压缩包 + 导入
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        public async Task<ApiResponse<Post>> Upload([FromForm] String Categoryname,[FromForm]String Parent,
            [FromForm] List<string> tags,IFormFile file,
            [FromForm]DateTime? publishTime,
            [FromServices] ICategoryService categoryService
        )
        {
            if (!file.FileName.EndsWith(".zip"))
            {
                return ApiResponse.BadRequest("只能上传zip格式的文件哦~");
            }
            var category = categoryService.Getbyname(Categoryname);
            int cid;
            Category categoryParent;
            Category c2;
            if (category == null)
            {
                Category c = new Category() { Name = Categoryname, Visible = true };
                cid = categoryService.AddCategory(c);
            }
            else
            {
                cid = category.Id;
            }
            if (Parent != null && !string.IsNullOrEmpty(Parent) && Parent != "null")
            {
                categoryParent = categoryService.GetbyParentname(Parent, cid);
                if(categoryParent != null)
                {
                    cid = categoryParent.Id;
                }
                else
                {
                    c2 = new Category() { Name = Parent, Visible = true, ParentId = cid };
                    cid = categoryService.AddCategory(c2);
                }
                try
                {
                    if (publishTime.HasValue && publishTime.Value > DateTime.Now)
                    {
                        return new ApiResponse<Post>()
                        {
                            Data = await _blogService.Upload(cid, tags,file,publishTime),
                            Message = "定时任务启动成功！"
                        };   
                    }
                    return new ApiResponse<Post>(await _blogService.Upload(cid, tags,file,null)); 
                }
                catch (Exception ex)
                {
                    return ApiResponse.Error($"解压文件出错：{ex.Message}");
                }
            }
            try
            {
                if (publishTime.HasValue && publishTime.Value > DateTime.Now)
                {
                    return new ApiResponse<Post>()
                    {
                        Data = await _blogService.Upload(cid, tags,file,publishTime),
                        Message = "定时任务启动成功！"
                    };   
                }
                return new ApiResponse<Post>(await _blogService.Upload(cid, tags,file,null)); 
            }
            catch (Exception ex)
            {
                return ApiResponse.Error($"解压文件出错：{ex.Message}");
            }
        }
    }
}
