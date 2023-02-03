using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace Personalblog.Services
{
    /// <summary>
    /// 图片库服务
    /// </summary>
    public class PiCLibService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Random _random;
        public List<string> ImageList { get; set; } = new();
        public PiCLibService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _random = Random.Shared;

            var impostPath = Path.Combine(_environment.WebRootPath, "media", "yasuo");
            var root = new DirectoryInfo(impostPath);
            foreach(var file in root.GetFiles())
            {
                ImageList.Add(file.FullName);
            }
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
            await using var fileStream = new FileStream(imagePath, FileMode.Open);
            var (image, format) = await Image.LoadWithFormatAsync(fileStream);

            //尺寸超出原图片尺寸，放大
            if(width > image.Width && height > image.Height)
            {
                image.Mutate(a => a.Resize(width, height));
            }
            else if(width > image.Width || height > image.Height)
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
            if(cropHeight > image.Height)
            {
                cropHeight = image.Height;
                cropWidth = (int)(image.Height / scleHeight * scaleWidth);
            }

            var cropRect = new Rectangle((image.Width - cropWidth) / 2, (image.Height - cropHeight) / 2, cropWidth, cropHeight);
            image.Mutate(a => a.Crop(cropRect));
            image.Mutate(a => a.Resize(width, height));

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
            return await GenerateSizedImageAsync(imagePath, width, height);
        }
    }
}
