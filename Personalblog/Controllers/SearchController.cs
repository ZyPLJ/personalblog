using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.ViewModels;

namespace Personalblog.Controllers
{
    public class SearchController : Controller
    {
        private readonly MyDbContext _context;
        public SearchController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Blog(string keyword, int categoryId = 0, int page = 1, int pageSize = 5)
        {
            var posts = _context.posts
                .Where(a => a.Title!.Contains(keyword))
                .Include(a => a.Categories)
                .ToList();
            return View("Result", new SearchResultViewModel
            {
                Keyword = keyword,
                Posts = posts
            });
        }
    }
}
