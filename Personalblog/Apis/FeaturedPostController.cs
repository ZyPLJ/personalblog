using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using PersonalblogServices.FPost;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    /// <summary>
    /// 推荐博客
    /// </summary>
    [ApiController]
    [Route("Api/[controller]")]
    public class FeaturedPostController : ControllerBase
    {
        private readonly IFPostService _fpostService;
        public FeaturedPostController(IFPostService fpostService)
        {
            _fpostService = fpostService;
        }
        [HttpGet]
        public ApiResponse<List<FeaturedPost>> GetList()
        {
            return new ApiResponse<List<FeaturedPost>>(_fpostService.GetList());
        }

        [HttpGet("{id:int}")]
        public ApiResponse<FeaturedPost> Get(int id)
        {
            var item = _fpostService.GetFeatures(id);
            return item == null ? ApiResponse.NotFound() : new ApiResponse<FeaturedPost>(item);
        }
        [HttpDelete("{id:int}")]
        public ApiResponse Delete(int id)
        {
            var item = _fpostService.GetFeatures(id);
            if (item == null) return ApiResponse.NotFound($"推荐博客记录 {id} 不存在");
            var rows = _fpostService.Delete(item);
            return ApiResponse.Ok($"deleted {rows} rows.");
        }
    }
}
