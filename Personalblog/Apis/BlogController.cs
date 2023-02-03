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
        public ApiResponse<Post> GetTopOnePost()
        {
            return new ApiResponse<Post> { Data = _blogService.GetTopOnePost() };
        }
        /// <summary>
        /// 上传博客压缩包 + 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ApiResponse<Post>> Upload([FromForm] String Categoryname, IFormFile file,
            [FromServices] ICategoryService categoryService
        )
        {
            if (!file.FileName.EndsWith(".zip"))
            {
                return ApiResponse.BadRequest("只能上传zip格式的文件哦~");
            }
            var category = categoryService.Getbyname(Categoryname);
            if (category == null)
            {
                Category c = new Category() { Name = Categoryname, Visible = true };
                int Cid = categoryService.AddCategory(c);
                try
                {
                    return new ApiResponse<Post>(await _blogService.Upload(Cid, file));
                }
                catch (Exception ex)
                {
                    return ApiResponse.Error($"解压文件出错：{ex.Message}");
                }
            }
            try
            {
                return new ApiResponse<Post>(await _blogService.Upload(category.Id, file));
            }
            catch (Exception ex)
            {
                return ApiResponse.Error($"解压文件出错：{ex.Message}");
            }
        }
    }
}
