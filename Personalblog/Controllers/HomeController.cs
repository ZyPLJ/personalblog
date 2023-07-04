using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.ViewModels;
using Personalblog.Models;
using PersonalblogServices.FCategory;
using PersonalblogServices.FPhoto;
using PersonalblogServices.FPost;
using PersonalblogServices.FtopPost;
using PersonalblogServices.Links;
using System.Diagnostics;
using Personalblog.Model.Entitys;
using PersonalblogServices.Articels;
using PersonalblogServices.Notice;
using X.PagedList;
using Messages = Personalblog.Contrib.SiteMessage.Messages;

namespace Personalblog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFPhotoService _fPhotoService;
        private readonly IFCategoryService _fCategoryService;
        private readonly ITopPostService _TopPostService;
        private readonly IFPostService _PostService;
        private readonly ILinkService _linkService;
        private readonly INoticeService _noticeService;
        private readonly IArticelsService _articelsService;
        public HomeController(ILogger<HomeController> logger,IFPhotoService fPhotoService,
            IFCategoryService fCategoryService,ITopPostService topPostService,
            IFPostService fPostService,ILinkService linkService,
            INoticeService noticeService,IArticelsService articelsService)
        {
            _logger = logger;
            _fPhotoService = fPhotoService;
            _fCategoryService = fCategoryService;
            _TopPostService = topPostService;
            _PostService = fPostService;
            _linkService = linkService;
            _noticeService = noticeService;
            _articelsService = articelsService;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel homeView = new HomeViewModel()
            {
                FeaturedPhotos = _fPhotoService.GetFeaturePhotos(),
                FeaturedCategories = _fCategoryService.GetFeaturedCategories(),
                TopPost = _TopPostService.GetTopOnePost(),
                FeaturedPosts = await _PostService.GetFeaturedPostsAsync(new QueryParameters
                {
                    Page = 1,
                    PageSize = 8,
                }),
                Links = await _linkService.GetAll(),
                Notices = await _noticeService.GetAllAsync(),
                FirstLastPost =await _articelsService.FirstLastPostAsync(),
                // MaxPost = await _articelsService.MaxPostAsync()
            };
            return View(homeView);
        }

        public async Task<IActionResult> GetFeaturedPosts(int page = 2, int pageSize = 8)
        {
            IPagedList<Post> data = await _PostService.GetFeaturedPostsAsync(new QueryParameters
            {
                Page = page,
                PageSize = pageSize,
            });
            if (data.Count == 0) {
                // 没有更多数据了，返回错误
                return NoContent();
            }
        
            return PartialView("Widgets/FeaturedPostCard", data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}