using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Categories;
using PersonalblogServices.Categorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.Categorys
{
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
            //List<Category> categories = _myDbContext.categories.ToList();
            //foreach (var item in categories)
            //{
            //    item.Posts = _myDbContext.posts.Where(p => p.CategoryId == item.Id).ToList();
            //}
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
        public Category? GetById(int id)
        {
            return _myDbContext.categories.FirstOrDefault(a => a.Id == id);
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
            return _myDbContext.categories.Where(a=>a.Name==name).FirstOrDefault();
        }
    }
}
