using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Services;

namespace Personalblog.Apis
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class DataAcqController : ControllerBase
    {
        private readonly CrawlService _crawlService;
        public DataAcqController(CrawlService crawlService)
        {
            _crawlService = crawlService;
        }
        [HttpGet]
        public async Task<string> Poem()
        {
            return await _crawlService.GetPoem();
        }

        [HttpGet]
        public async Task<string> Hitokoto()
        {
            return await _crawlService.GetHitokoto();
        }
    }
}
