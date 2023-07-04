using Microsoft.AspNetCore.Mvc;
using Personalblog.Contrib.SiteMessage;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Services;
using Messages = Personalblog.Contrib.SiteMessage.Messages;

namespace Personalblog.Controllers
{
    public class PhotoController : Controller
    {
        private readonly PhotoService _photoService;
        private readonly Messages _messages;
        public PhotoController(PhotoService photoService, Messages messages)
        {
            _photoService = photoService;
            _messages = messages;
        }

        public async Task<IActionResult> Index(long? yes,int page = 1,int pageSize=10)
        {
            PhotographyViewModel photography = new PhotographyViewModel()
            {
                photos = await _photoService.GetPageList(page, pageSize)
            };
            return View(photography);
        }
        [Route("Photo/Details/{id}.html")]
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
                _messages.Error("当前没有图片，请先上传！");
                return RedirectToAction("Index", "Home");
            }
            _messages.Info($"随机推荐了图片 <b>{item.Title}</b> 给你~");
            return RedirectToAction(nameof(Details), new { id = item.Id });
        }
        public async Task<IActionResult> Next(string id)
        {
            var item = await _photoService.GetNext(id);
            if (item == null)
            {
                _messages.Warning("没有下一张图片了~");
                return RedirectToAction(nameof(Details), new { id });
            }

            return RedirectToAction(nameof(Details), new { id = item.Id });
        }

        public async Task<IActionResult> Previous(string id)
        {
            var item = await _photoService.GetPrevious(id);
            if (item == null)
            {
                _messages.Warning("没有上一张图片了~");
                return RedirectToAction(nameof(Details), new { id });
            }

            return RedirectToAction(nameof(Details), new { id = item.Id });
        }
    }
}
