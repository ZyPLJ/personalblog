using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Models;
using Personalblog.Model.ViewModels;
using PersonalblogServices.Articels;
using PersonalblogServices.Articels.Dto;
using PersonalblogServices.Categorys;

namespace Personalblog.Controllers
{
    public class BlogController : Controller
    {
        public ICategoryService CategoryService { get; set; }
        public IArticelsService ArticelsService { get; set; }
        public BlogController(ICategoryService categoryService,IArticelsService articelsService)
        {
            this.CategoryService = categoryService;
            this.ArticelsService = articelsService;
        }
        /// <summary>
        /// 所有文章列表
        /// </summary>
        /// <param name="categoryId">文章类别id 默认是.net文章</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">页面最大展示数据的数量</param>
        /// <returns></returns>
        public IActionResult List(int categoryId = 3, int page = 1, int pageSize = 2)
        {
            var clist = CategoryService.categories();
            BlogListViewModel blogList = new BlogListViewModel()
            {
                //CurrentCategory = categoryId == 0 ? clist[0] : clist.First(c => c.Id == categoryId),
                CurrentCategoryId = categoryId,
                Categories = clist,
                Posts = ArticelsService.GetPagedList(new QueryParameters
                {
                    Page = page,
                    PageSize = pageSize,
                    CategoryId = categoryId,
                })
            };
            return View(blogList);
        }
        /// <summary>
        /// 根据文章id去查看文章
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IActionResult Post(string id)
        {
            try
            {
                var post = ArticelsService.GetArticels(id);
                return View(post);
            }
            catch (Exception)
            {
                return Content(id);
                throw;
            }
        }
        /// <summary>
        /// 随机一篇文章
        /// </summary>
        /// <returns></returns>
        public IActionResult RandomPost()
        {
            var posts = ArticelsService.GetPhotos();
            var randPost = posts[new Random().Next(posts.Count)];
            TempData["msg"] = $"随机推荐了文章<b>{randPost.Title}</b>给你~";
            return RedirectToAction(nameof(Post), new { id = randPost.Id });
        }
    }
}
