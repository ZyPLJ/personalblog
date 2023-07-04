using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using PersonalblogServices.Categorys;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    /// <summary>
    /// 推荐分类
    /// </summary>
    /// 
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class FeaturedCategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public FeaturedCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [AllowAnonymous]
        [HttpGet]
        public ApiResponse<List<FeaturedCategory>> GetAll()
        {
            return new ApiResponse<List<FeaturedCategory>>(_categoryService.GetFeaturedCategories());
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]

        public ApiResponse<FeaturedCategory> Get(int id)
        {
            var item = _categoryService.GetFeaturedCategoryById(id);
            return item == null
                ? ApiResponse.NotFound($"推荐分类记录 {id} 不存在")
                : new ApiResponse<FeaturedCategory>(item);
        }
        [HttpDelete("{id:int}")]
        public ApiResponse Delete(int id)
        {
            var item = _categoryService.GetFeaturedCategoryById(id);
            if (item == null) return ApiResponse.NotFound($"推荐分类记录 {id} 不存在");
            var rows = _categoryService.DeleteFeaturedCategoryById(id);
            return ApiResponse.Ok($"deleted {rows} rows.");
        }
    }
}
