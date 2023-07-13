using Microsoft.AspNetCore.Mvc;
using Personalblog.Migrate;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Services;
using Messages = Personalblog.Contrib.SiteMessage.Messages;

namespace Personalblog.Controllers
{
    public class ArticleController : Controller
    {
        //注入服务
        private readonly MyDbContext _myDbContext;
        private readonly PhotoService _photoService;
        private readonly Messages _messages;
        public ArticleController(MyDbContext myDbContext, PhotoService photoService,
            Messages messages)
        {
            _myDbContext = myDbContext;
            _photoService = photoService;
            _messages = messages;
        }
        
        public IActionResult Init()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Init([FromServices] ConfigService conf,InitViewModel vm)
        {
            if (conf.GetAll().Any())
            {
                _messages.Error("已经完成初始化！");
                return RedirectToAction("Index","Home");
            }
            
            if (!ModelState.IsValid) return View();
            
            //保存信息
            //todo 这里暂时存储明文密码，后期要换成MD5加密存储
            _myDbContext.users.Add(new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = vm.Username,
                Password = vm.Password
            });
            _myDbContext.configItems.Add(new ConfigItem()
            {
                Description = "初始化",
                Key = "host",
                Value = vm.Host,
                IsShowComment = true
            });
            _myDbContext.SaveChanges();

            _messages.Success("初始化完成！");
            return RedirectToAction("Index","Home");
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
