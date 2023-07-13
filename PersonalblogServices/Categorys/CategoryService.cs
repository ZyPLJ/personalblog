using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Categories;

namespace PersonalblogServices.Categorys
{
    //后期将推荐分类方法转移到FCategory中去
    public class CategoryService:ICategoryService
    {
        private readonly MyDbContext _myDbContext;
        private readonly IMapper _mapper;

        public CategoryService(MyDbContext myDbContext,IMapper mapper)
        {
            _myDbContext = myDbContext;
            _mapper = mapper;
        }

        public List<Category> categories()
        {
            List<Category> categories = _myDbContext.categories.Include("Posts").ToList();
            return categories;
        }

        public List<object> GetWordCloud()
        {
            var list = _myDbContext.
                categories.Include(a => a.Posts).ToList();
            var data = new List<object>();
            foreach (var item in list)
            {
                data.Add(new { name = item.Name, value = item.Posts.Count });
            }
            return data;
        }
        public async Task<Category?> GetById(int id)
        {
            return await _myDbContext.categories.FirstOrDefaultAsync(a => a.Id == id);
        }
        public FeaturedCategory AddOrUpdateFeaturedCategory(Category category, FeaturedCategoryCreationDto dto)
        {
            var item = _myDbContext.featuredCategories.FirstOrDefault(a => a.CategoryId == category.Id);
            if (item == null)
            {
                item = new FeaturedCategory
                {
                    CategoryId = category.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    IconCssClass = dto.IconCssClass
                };
            }
            else
            {
                item.Name = dto.Name;
                item.Description = dto.Description;
                item.IconCssClass = dto.IconCssClass;
            }
            _myDbContext.featuredCategories.Add(item);
            _myDbContext.SaveChanges();
            return item;
        }

        public List<Category> GetAll()
        {
            return _myDbContext.categories.ToList();
        }

        public int SetVisibility(Category category, bool isVisible)
        {
            category.Visible = isVisible;
            _myDbContext.categories.Update(category);
            return _myDbContext.SaveChanges();
        }

        public int DeleteFeaturedCategory(Category category)
        {

            var item = _myDbContext.featuredCategories.First(a => a.CategoryId == category.Id);
            _myDbContext.Remove(item);
            return item == null ? 0 : _myDbContext.SaveChanges();
        }
        public List<FeaturedCategory> GetFeaturedCategories()
        {
            return _myDbContext.featuredCategories.Include(a=>a.Category).ToList();
        }
        public FeaturedCategory? GetFeaturedCategoryById(int id)
        {
            return _myDbContext.featuredCategories.Where(a=>a.Id==id)
                .Include(a=>a.Category).First();
        }
        public int DeleteFeaturedCategoryById(int id)
        {
            var item = _myDbContext.featuredCategories.First(a => a.Id == id);
            _myDbContext.Remove(item);
            return _myDbContext.SaveChanges();
        }
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int AddCategory(Category category)
        {
            try
            {
                _myDbContext.Add(category);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _myDbContext.SaveChanges();
            return category.Id;
        }

        public Category Getbyname(string name)
        {
            return _myDbContext.categories.Where(a=>a.Name==name && a.ParentId == 0).FirstOrDefault();
        }

        public Category GetbyParentname(string name, int id)
        {
            return _myDbContext.categories.Where(a => a.Name == name && a.ParentId == id).FirstOrDefault();
        }

        public async Task<bool> CategoryGetPost(int id)
        {
             var post = await _myDbContext.posts.Where(p => p.CategoryId == id).AnyAsync();
             var category = await _myDbContext.categories.Where(c => c.ParentId == id).AnyAsync();
             if (post || category)
                 return true;
             else return false;
        }

        public async Task<int> DeleteAsync(Category category)
        {
             _myDbContext.categories.Remove(category);
             return await _myDbContext.SaveChangesAsync();
        }

        public async Task<Category> Update(Category category)
        {
            _myDbContext.categories.Update(category);
            await _myDbContext.SaveChangesAsync();
            return category;
        }
    }
}
