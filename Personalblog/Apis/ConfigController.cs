using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Services;

namespace Personalblog.Apis
{
    /// <summary>
    /// 配置中心
    /// </summary>
    [ApiController]
    [Route("Api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ConfigService _service;
        public ConfigController(ConfigService service)
        {
            _service = service;
        }
        [HttpGet]
        public List<ConfigItem> GetAll()
        {
            return _service.GetAll();
        }
    }
}
