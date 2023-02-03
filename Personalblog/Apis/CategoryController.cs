using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Categories;
using PersonalblogServices.Categorys;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    [ApiController]
    [Route("Api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public List<object> WordCloud()
        {
            return _categoryService.GetWordCloud();
        }
        /// <summary>
        /// 设置为推荐分类
        /// </summary>
        /// <seealso href="https://fontawesome.com/search?m=free">FontAwesome图标库</seealso>
        /// <param name="id"></param>
        /// <param name="dto">推荐信息 <see cref="FeaturedCategoryCreationDto"/></param>
        /// <returns></returns>
        [HttpPost("{id:int}/[action]")]
        public ApiResponse<FeaturedCategory> SetFeatured(int id, [FromBody] FeaturedCategoryCreationDto dto)
        {
            var item = _categoryService.GetById(id);
            return item == null
                ? ApiResponse.NotFound($"分类 {id} 不存在")
                : new ApiResponse<FeaturedCategory>(_categoryService.AddOrUpdateFeaturedCategory(item, dto));
        }
        [AllowAnonymous]
        [HttpGet("All")]
        public ApiResponse<List<Category>> GetAll()
        {
            return new ApiResponse<List<Category>>(_categoryService.GetAll());
        }
        /// <summary>
        /// 设置分类可见
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id:int}/[action]")]
        public ApiResponse SetVisible(int id)
        {
            var item = _categoryService.GetById(id);
            if (item == null) return ApiResponse.NotFound($"分类 {id} 不存在");
            var rows = _categoryService.SetVisibility(item, true);
            return ApiResponse.Ok($"affect {rows} rows.");
        }
        /// <summary>
        /// 设置分类不可见
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id:int}/[action]")]
        public ApiResponse SetInvisible(int id)
        {
            var item = _categoryService.GetById(id);
            if (item == null) return ApiResponse.NotFound($"分类 {id} 不存在");
            var rows = _categoryService.SetVisibility(item, false);
            return ApiResponse.Ok($"affect {rows} rows.");
        }
        /// <summary>
        /// 取消推荐分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id:int}/[action]")]
        public ApiResponse CancelFeatured(int id)
        {
            var item = _categoryService.GetById(id);
            if (item == null) return ApiResponse.NotFound($"分类 {id} 不存在");
            var rows = _categoryService.DeleteFeaturedCategory(item);
            return ApiResponse.Ok($"delete {rows} rows.");
        }
    }
}
