using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    /// <summary>
    /// 推荐图片
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class FeaturedPhotoController : ControllerBase
    {
        private readonly Personalblog.Services.PhotoService _photoService;
        private readonly MyDbContext _myDbContext;
        public FeaturedPhotoController(
            Personalblog.Services.PhotoService photoService,
            MyDbContext myDbContext)
        {
            _photoService = photoService;
            _myDbContext = myDbContext;
        }
        [AllowAnonymous]
        [HttpGet]
        public ApiResponse<List<FeaturedPhoto>> GetList()
        {
            return new ApiResponse<List<FeaturedPhoto>>(
                _myDbContext.featuredPhotos.Include(a => a.Photo).ToList());
        }
        [HttpDelete("{id:int}")]
        public ApiResponse Delete(int id)
        {
            var item = _myDbContext.featuredPhotos.FirstOrDefault(a => a.Id == id);
            if(item == null) return ApiResponse.NotFound($"推荐图片记录 {id} 不存在");
            _myDbContext.Remove(item);
            int result = _myDbContext.SaveChanges();
            return ApiResponse.Ok($"deleted {result} rows.");
        }
    }
}
