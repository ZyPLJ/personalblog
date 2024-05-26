using Newtonsoft.Json.Linq;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Personalblog.Services
{
    /// <summary>
    /// 图片库服务
    /// </summary>
    public class PiCLibService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Random _random = new Random();
        public List<string> ImageList { get; set; } = new();
        public List<string> ImageListTop { get; set; } = new();
        public PiCLibService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _random = Random.Shared;
            ImageList = LoadImageList("media/yasuo");
            ImageListTop = LoadImageList("media/Top");
        }
        private List<string> LoadImageList(string imagePath)
        {
            var imageList = new List<string>();
            var impostPath = Path.Combine(_environment.WebRootPath, imagePath);
            var root = new DirectoryInfo(impostPath);
            foreach (var file in root.GetFiles())
            {
                imageList.Add(file.FullName);
            }
            return imageList;
        }
        /// <summary>
        /// 求最大公约数
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns>最大公约数</returns>
        private static int GetGreatestCommonDivisor(int m,int n)
        {
            if (m < n) (n, m) = (m, n);
            while (n != 0)
            {
                var r = m % n;
                m = n;
                n = r;
            }
            return m;
        }
        private static (double,double) GetPhotoScale(int width,int height)
        {
            if(width == height) return (1,1);
            var gcd = GetGreatestCommonDivisor(width,height);
            return ((double)width / gcd, (double)height / gcd);
        }
        /// <summary>
        /// 生成指定尺寸图片
        /// </summary>
        /// <param name="imagePath">图片位置</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        async Task<(Image,IImageFormat)> GenerateSizedImageAsync(string imagePath,int width,int height)
        {
            using var fileStream = new FileStream(imagePath, FileMode.Open);
            var (image, format) = await Image.LoadWithFormatAsync(fileStream);

            //尺寸超出原图片尺寸，放大
            if (width > image.Width && height > image.Height)
            {
                image.Mutate(a => a.Resize(width, height));
            }
            else if (width > image.Width || height > image.Height)
            {
                // 改变比例大的边
                if (width / image.Width < height / image.Height)
                    image.Mutate(a => a.Resize(0, height));
                else
                    image.Mutate(a => a.Resize(width, 0));
            }

            //将输入的尺寸作为裁剪比例
            var (scaleWidth, scleHeight) = GetPhotoScale(width, height);
            var cropWidth = image.Width;
            var cropHeight = (int)(image.Width / scaleWidth * scleHeight);
            if (cropHeight > image.Height)
            {
                cropHeight = image.Height;
                cropWidth = (int)(image.Height / scleHeight * scaleWidth);
            }



            var cropRect = new Rectangle((image.Width - cropWidth) / 2, (image.Height - cropHeight) / 2, cropWidth, cropHeight);
            image.Mutate(a => a.Crop(cropRect));
            image.Mutate(a => a.Resize(width, height));
            return (image, format);

        }

        async Task<(Image, IImageFormat)> GenerateSizedImageAsyncOd(string imagePath)
        {
            using var fileStream = new FileStream(imagePath, FileMode.Open);
            var (image, format) = await Image.LoadWithFormatAsync(fileStream);
            return (image, format);
        }
        /// <summary>
        /// 从图片文件夹获取随机图片
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public async Task<(Image, IImageFormat)> GetRandomImageAsync(int width,int height,string? seed = null)
        {
            var rnd = seed == null ? _random : new Random(seed.GetHashCode());
            var imagePath = ImageList[rnd.Next(0, ImageList.Count)];
            var (image, format) = await GenerateSizedImageAsync(imagePath, width, height);
            return (image, format);
        }
        public async Task<(Image, IImageFormat)> GetRandomImageAsyncTop(string? seed = null)
        {
            var rnd = _random;
            var imagePath = ImageListTop[rnd.Next(0, ImageListTop.Count)];
            return await GenerateSizedImageAsyncOd(imagePath);
        }

        public async Task<List<string>> GetQiliuImageAsyncTop()
        {
            var imagePaths = await Task.Run(() =>
            {
                var rnd = new Random();
                return ImageListTop.OrderBy(x => rnd.Next()).Take(3)
                    .Select(imagePath =>
                    {
                        var fileName = Path.GetFileName(imagePath);
                        return Path.Combine("https://cdn.pljzy.top", "Top", fileName);
                    })
                    .ToList();
            });

            return imagePaths;
        }
        public async Task<(Image, IImageFormat)> GetDesignImageAsync(int width,int height,string seed)
        {
            var imagePath = Path.Combine(_environment.WebRootPath, $"media/Tool/{seed}");
            var (image, format) = await GenerateSizedImageAsync(imagePath, width, height);
            return (image, format);
        }
    }
}
