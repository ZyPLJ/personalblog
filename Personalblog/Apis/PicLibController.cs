using Microsoft.AspNetCore.Mvc;
using Personalblog.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
            
            var font = SystemFonts.CreateFont("DejaVu Sans", 50);
            
            var location = new PointF(image.Width - 250, image.Height - 100);
            image.Mutate(ctx => ctx.DrawText("ZY blog", font, new Rgba32(255, 255, 255, 128), location));
            
            # region 水印图像
            // 创建水印图像
            // var watermarkImage = new Image<Rgba32>(image.Width, image.Height);
            // watermarkImage.Mutate(x => x.BackgroundColor(new Rgba32(0, 0, 0, 0)));
            // var font = SystemFonts.CreateFont("Arial", 50, FontStyle.Bold);
            // var location = new PointF(watermarkImage.Width - 250, watermarkImage.Height - 100);
            // watermarkImage.Mutate(x => x.DrawText("ZY blog", font, new Rgba32(255, 255, 255, 128), location));

            // 将水印图像与原始图像叠加
            // image.Mutate(x => x.DrawImage(watermarkImage, 1f));
            #endregion
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
