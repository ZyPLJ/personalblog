using Microsoft.AspNetCore.Mvc;
using Personalblog.Migrate;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Services;

namespace Personalblog.Controllers
{
    public class ArticleController : Controller
    {
        //注入服务
        private readonly MyDbContext _myDbContext;
        private readonly PhotoService _photoService;
        public ArticleController(MyDbContext myDbContext, PhotoService photoService)
        {
            _myDbContext = myDbContext;
            _photoService = photoService;
        }

        public IActionResult Index()
        {
            CreateMd createMd = new CreateMd();
            createMd.C(_myDbContext);
            return View();
        }
        public List<Photo> BatchImport()
        {
             var result =  _photoService.Import();
            return result;
        }
        public IActionResult Chart()
        {
            return View();
        }
    }
}
