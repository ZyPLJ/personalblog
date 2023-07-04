using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Services;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    /// <summary>
    /// 配置中心
    /// </summary>
    [Authorize]
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

        [HttpGet("{id:int}")]
        public async Task<ApiResponse> IsShow(int id)
        {
            return await _service.IsShow(id);
        }
        [HttpDelete("{id:int}")]
        public async Task<ApiResponse> Del(int id)
        {
            return await _service.DelAsync(id);
        }

        [HttpPost]
        public async Task<ApiResponse> Add(ConfigItem configItem)
        {
            return await _service.AddAsync(configItem);
        }
    }
}
