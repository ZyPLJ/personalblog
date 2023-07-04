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
    public interface ICategoryService
    {
        //查询所有文章分类
        List<Category> categories();
        //生成分类词云数据
        List<object> GetWordCloud();
        Task<Category?> GetById(int id);
        FeaturedCategory AddOrUpdateFeaturedCategory(Category category, FeaturedCategoryCreationDto dto);
        List<Category> GetAll();
        int SetVisibility(Category category, bool isVisible);
        int DeleteFeaturedCategory(Category category);
        List<FeaturedCategory> GetFeaturedCategories();
        FeaturedCategory? GetFeaturedCategoryById(int id);
        int DeleteFeaturedCategoryById(int id);
        //添加分类
        int AddCategory(Category category);
        //更加分类名称去查询
        Category Getbyname(string name);
        //查询二级目录
        Category GetbyParentname(string name, int id);
        //更据分类id查询文章查询是否存在
        Task<bool> CategoryGetPost(int id);
        //更据实体删除分类
        Task<int> DeleteAsync(Category category);
        //修改分类
        Task<Category> Update(Category category);
    }
}
