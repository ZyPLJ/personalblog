using Microsoft.AspNetCore.Mvc;
using Personalblog.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;

namespace Personalblog.Apis
{
    public class PicLibController : Controller
    {
        private readonly PiCLibService _service;
        private readonly IConfiguration _configuration;
        public PicLibController(PiCLibService service,IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }
        private static async Task<IActionResult> GenerateImageResponse(Image image, IImageFormat format)
        {
            var encoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(format);
            await using var stream = new MemoryStream();
            await image.SaveAsync(stream, encoder);
            return new FileContentResult(stream.GetBuffer(), "image/jpeg");
        }
        /// <summary>
        /// 指定尺寸随机图片
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        [HttpGet("Random/{width:int}/{height:int}")]
        public async Task<IActionResult> GetRandomImage(int width,int height)
        {
            var (image,format) = await _service.GetRandomImageAsync(width,height);
            return await GenerateImageResponse(image,format);
        }
        [HttpGet("RandomTop/{seed}/")]
        public async Task<IActionResult> GetRandomImageTop(string seed)
        {
            var (image,format) = await _service.GetRandomImageAsyncTop(seed);
            return await GenerateImageResponse(image,format);
        }

        [HttpGet]
        public async Task<string> GetRandomImageTopQiliu(string? seed)
        {
            string path = await _service.GetQiliuImageAsyncTop();
            return path;
        }
        /// <summary>
        /// 指定尺寸随机图片 (带初始种子)
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [HttpGet("Random/{seed}/{width:int}/{height:int}")]
        public async Task<IActionResult> GetRandomImage(string seed, int width, int height)
        {
            var (image, format) = await _service.GetRandomImageAsync(width, height, seed);
            return await GenerateImageResponse(image, format);
        }
    }
}
