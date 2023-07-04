using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Personalblog.Controllers;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Categories;

namespace Personalblog.Services
{
    public class CategoryService
    {
        private readonly MyDbContext _myDbContext;
        private readonly LinkGenerator _generator;
        private readonly IHttpContextAccessor _accessor;
        public CategoryService(MyDbContext myDbContext,
            LinkGenerator generator, IHttpContextAccessor accessor)
        {
            _myDbContext = myDbContext;
            _generator = generator;
            _accessor = accessor;
        }
        /// <summary>
        /// 生成文章分类树
        /// </summary>
        public List<CategoryNode> GetNodes(int parentId = 0)
        {
            var categories = _myDbContext.categories
                .Where(a => a.ParentId == parentId)
                .Include(a=>a.Posts)
                .ToList();
            if (categories.Count == 0) return null;

            return categories.Select(c => new CategoryNode
            {
                text = c.Name,
                href = _generator.GetUriByAction(
                _accessor.HttpContext!,
                nameof(BlogController.List),
                "Blog",
                new { categoryId = c.Id },"https"
                ),
                tags = new List<string> { c.Posts.Count.ToString() },
                nodes = GetNodes(c.Id)
            }).ToList();
        }
    }
}
