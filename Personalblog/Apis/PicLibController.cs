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
        private readonly QiniuService _qiniuService;
        public PicLibController(PiCLibService service,IConfiguration configuration,
            QiniuService qiniuService)
        {
            _service = service;
            _configuration = configuration;
            _qiniuService = qiniuService;
        }
        private static async Task<IActionResult> GenerateImageResponse(Image image, IImageFormat format)
        {
            var encoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(format);
            using (var stream = new MemoryStream())
            {
                //windows字体 Arial
                //liunx字体 DejaVu Sans
                var font = SystemFonts.CreateFont("Arial", 50);

                var location = new PointF(image.Width - 250, image.Height - 100);
                image.Mutate(ctx => ctx.DrawText("ZY blog", font, new Rgba32(255, 255, 255, 128), location));

                #region 水印图像
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
                try
                {
                    return new FileContentResult(stream.GetBuffer(), "image/jpeg");
                }
                finally
                {
                    image.Dispose();
                }
            }
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
            var (image, format) = await _service.GetRandomImageAsync(width, height);
            try
            {
                return await GenerateImageResponse(image, format);
            }
            finally
            {
                image.Dispose();
            }
        }
        [HttpGet("RandomTop/{seed}/")]
        public async Task<IActionResult> GetRandomImageTop(string seed)
        {
            var (image,format) = await _service.GetRandomImageAsyncTop(seed);
            try
            {
                return await GenerateImageResponse(image, format);
            }
            finally
            {
                image.Dispose();
            }
        }

        [HttpGet]
        public async Task<List<string>> GetRandomImageTopQiliu(string? seed)
        {
            // string path = await _service.GetQiliuImageAsyncTop();
            // string path = await _qiniuService.GetRandomImageAsync();
            return await _service.GetQiliuImageAsyncTop();
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
            try
            {
                return await GenerateImageResponse(image, format);
            }
            finally
            {
                image.Dispose();
            }
        }
        /// <summary>
        /// 返回指定图片
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        [HttpGet("Design/{width:int}/{height:int}")]
        public async Task<IActionResult> GetDesignImage(string seed,int width,int height)
        {
            var (image, format) = await _service.GetDesignImageAsync(width, height, seed);
            try
            {
                return await GenerateImageResponse(image, format);
            }
            finally
            {
                image.Dispose();
            }
        }
    }
}
