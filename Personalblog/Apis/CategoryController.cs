using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Categories;
using PersonalblogServices.Categorys;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    [ApiController]
    [Authorize]
    [Route("Api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService,IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
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
        public async Task<ApiResponse<FeaturedCategory>> SetFeatured(int id, [FromBody] FeaturedCategoryCreationDto dto)
        {
            var item =await _categoryService.GetById(id);
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
        public async Task<ApiResponse> SetVisible(int id)
        {
            var item =await _categoryService.GetById(id);
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
        public  async Task<ApiResponse> SetInvisible(int id)
        {
            var item =await _categoryService.GetById(id);
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
        public async Task<ApiResponse> CancelFeatured(int id)
        {
            var item =await _categoryService.GetById(id);
            if (item == null) return ApiResponse.NotFound($"分类 {id} 不存在");
            var rows = _categoryService.DeleteFeaturedCategory(item);
            return ApiResponse.Ok($"delete {rows} rows.");
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var item = await _categoryService.GetById(id);
            if (item == null) return ApiResponse.NotFound();

            if (await _categoryService.CategoryGetPost(id))
                return ApiResponse.BadRequest("所选分类下有文章或者分类有二级目录，不能删除！");

            var rows =await _categoryService.DeleteAsync(item);
            return ApiResponse.Ok($"已删除 {rows} 条数据");
        }

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<Category>> Update(int id, [FromBody] CategoryCreationDto dto)
        {
            var item = await _categoryService.GetById(id);
            if (item == null) return ApiResponse.NotFound();

            item = _mapper.Map(dto, item);
            return new ApiResponse<Category>(await _categoryService.Update(item));
        }
        [HttpGet("{id:int}")]
        public async Task<ApiResponse<Category>> GetCategory(int id)
        {
            var item = await _categoryService.GetById(id);
            return new ApiResponse<Category>(item);
        }
    }
}
