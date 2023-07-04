using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using PersonalblogServices.FPost;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    /// <summary>
    /// 推荐博客
    /// </summary>
    [Authorize]
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
        public async Task<ApiResponse<List<FeaturedPost>>> GetList()
        {
            return new ApiResponse<List<FeaturedPost>>(await _fpostService.GetListAsync());
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
        [HttpPut("{id:int}/{newSortOrder:int}")]
        public async Task<ApiResponse> UpdateSort(int id, int newSortOrder)
        {
            try
            {
                var result = await _fpostService.UpdateSortOrderAsync(id, newSortOrder);
                if (result)
                    return ApiResponse.Ok("修改排序成功！");
                return ApiResponse.Error("修改排序失败");
            }
            catch (Exception ex)
            {
                return ApiResponse.Error("发生错误：" + ex.Message);
            }
        }
    }
}
