using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Extensions;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Blog;
using Personalblog.Services;
using Personalblog.Utils;
using PersonalblogServices.Categorys;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    /// <summary>
    /// 文章
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class BlogPostController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly PostService _postService;
        private readonly BlogService _blogService;
        public BlogPostController(PostService postService, 
            BlogService blogService,
            IMapper mapper)
        {
            _postService = postService;
            _blogService = blogService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ApiResponse<Post> Get(string id)
        {
            var post = _postService.GetById(id);
            return post == null ? ApiResponse.NotFound() : new ApiResponse<Post>(post);
        }
        [AllowAnonymous]
        [HttpGet]
        public ApiResponsePaged<Post> GetList([FromQuery] PostQueryParameters param)
        {
            var pagedList = _postService.GetPagedList(param);
            return new ApiResponsePaged<Post>
            {
                Message = "Get posts list",
                Data = pagedList.ToList(),
                Pagination = pagedList.ToPaginationMetadata()
            };
        }
        [HttpDelete("{id}")]
        public ApiResponse Delete(string id)
        {
            var post = _postService.GetById(id);
            if (post == null) return ApiResponse.NotFound($"博客 {id} 不存在");
            var rows = _postService.Delete(post);
            return ApiResponse.Ok($"删除了 {rows} 篇博客");
        }
        /// <summary>
        /// 设置为推荐博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/[action]")]
        public ApiResponse<FeaturedPost> SetFeatured(string id)
        {
            var post = _postService.GetById(id);
            FeaturedPost f = new FeaturedPost();
            if (post == null)
            {
                return ApiResponse.NotFound();
            }
            f = _blogService.AddFeaturedPost(post);
            return new ApiResponse<FeaturedPost>(f);
        }
        /// <summary>
        /// 取消推荐博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/[action]")]
        public ApiResponse CancelFeatured(string id)
        {
            var post = _postService.GetById(id);
            if (post == null) return ApiResponse.NotFound($"博客 {id} 不存在");
            var rows = _blogService.DeleteFeaturedPost(post);
            return ApiResponse.Ok($"delete {rows} rows.");
        }
        /// <summary>
        /// 设置置顶（只能有一篇置顶）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/[action]")]
        public async Task<ApiResponse<TopPost>> SetTop(string id)
        {
            var post = _postService.GetById(id);
            if (post == null) return ApiResponse.NotFound($"博客 {id} 不存在");
            var (data, rows) = await _blogService.SetTopPostAsync(post);
            return new ApiResponse<TopPost> { Data = null, Message = $"ok. deleted {rows} old topPosts." };
        }
        [HttpPost]
        public async Task<ApiResponse<Post>> Add(PostCreationDto dto,
            [FromServices] ICategoryService categoryService)
        {
            var post = _mapper.Map<Post>(dto);
            var category =await categoryService.GetById(dto.CategoryId);
            if (category == null) return ApiResponse.BadRequest($"分类 {dto.CategoryId} 不存在！");

            post.Id = GuidUtils.GuidTo16String();
            post.CreationTime = DateTime.Now;
            post.LastUpdateTime = DateTime.Now;
            post.Categories = category;

            return new ApiResponse<Post>(await _postService.InsertOrUpdateAsync(post));
        }
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<ApiResponse<Post>> Update(string id, PostUpdateDto dto)
        {
            var post = _postService.GetById(id);
            if (post == null) return ApiResponse.NotFound($"博客 {id} 不存在");

            // mapper.Map(source) 得到一个全新的对象
            // mapper.Map(source, dest) 在 source 对象的基础上修改
            post = _mapper.Map(dto, post);
            post.LastUpdateTime = DateTime.Now;
            return new ApiResponse<Post>(await _postService.InsertOrUpdateAsync(post));
       }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("{id}/[action]")]
        public ApiResponse UploadImage(string id, IFormFile file)
        {
            var post = _postService.GetById(id);
            if (post == null)
            {
                return ApiResponse.NotFound($"博客 {id} 不存在");
            }
            var imgUrl = _postService.UploadImage(post, file);
            return ApiResponse.Ok(new
            {
                imgUrl,
                imgName = Path.GetFileNameWithoutExtension(imgUrl)
            });
        }
    }
}
