using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.ViewModels;
using Personalblog.Services;

namespace Personalblog.Controllers
{
    public class PhotoController : Controller
    {
        private readonly PhotoService _photoService;
        public PhotoController(PhotoService photoService)
        {
            _photoService = photoService;
        }

        public IActionResult Index(long? yes,int page = 1,int pageSize=10)
        {
            PhotographyViewModel photography = new PhotographyViewModel()
            {
                photos = _photoService.GetPageList(page, pageSize)
            };
            return View(photography);
        }
        public IActionResult Details(string id)
        {
            var photo = _photoService.GetById(id);
            if (photo == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }
        /// <summary>
        /// 随机获取一张图片
        /// </summary>
        /// <returns></returns>
        public IActionResult RandomPhoto()
        {
            var item = _photoService.GetRandomPhoto();
            if(item == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["msg"] = $"随机推荐了图片 <b>{item.Title}</b> 给你~";
            return RedirectToAction(nameof(Details), new { id = item.Id });
        }
    }
}
