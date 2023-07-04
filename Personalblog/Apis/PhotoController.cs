using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.Photography;
using PersonalblogServices;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    /// <summary>
    /// 摄影
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    [ApiExplorerSettings(GroupName = "blog")]
    public class PhotoController : ControllerBase
    {
        private readonly Personalblog.Services.PhotoService _photoService;
        public PhotoController(Personalblog.Services.PhotoService photoService)
        {
            _photoService = photoService;
        }
        [HttpGet]
        public async Task<ApiResponsePaged<Photo>> GetList(int page = 1, int pageSize = 10)
        {
            var paged = await _photoService.GetPageList(page, pageSize);
            return new ApiResponsePaged<Photo>
            {
                Pagination = paged.ToPaginationMetadata(),
                Data = paged.ToList()
            };
        }

        [Authorize]
        [HttpPost]
        public ApiResponse<Photo> Add([FromForm] PhotoCreationDto dto, IFormFile file)
        {
            var photo = _photoService.Add(dto, file);

            return !ModelState.IsValid
                ? ApiResponse.BadRequest(ModelState)
                : new ApiResponse<Photo>(photo);
        }
        /// <summary>
        /// 设置为推荐图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/[action]")]
        public ApiResponse<FeaturedPhoto> SetFeatured(string id)
        {
            var photo = _photoService.GetById(id);
            return photo == null
                ? ApiResponse.NotFound($"图片 {id} 不存在")
                : new ApiResponse<FeaturedPhoto>(_photoService.AddFeaturedPhoto(photo));
        }
        /// <summary>
        /// 取消推荐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/[action]")]
        public ApiResponse CancelFeatured(string id)
        {
            var photo = _photoService.GetById(id);
            if (photo == null) return ApiResponse.NotFound($"图片 {id} 不存在");
            var rows = _photoService.DeleteFeaturedPhoto(photo);
            return ApiResponse.Ok($"deleted {rows} rows.");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ApiResponse Delete(string id)
        {
            var photo = _photoService.GetById(id);
            if (photo == null) return ApiResponse.NotFound($"图片 {id} 不存在");
            var rows = _photoService.DeleteById(id);
            return rows > 0
                ? ApiResponse.Ok($"deleted {rows} rows.")
                : ApiResponse.Error("deleting failed.");
        }
    }
}
