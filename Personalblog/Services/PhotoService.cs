using Personalblog.Migrate;
using Personalblog.Model.Entitys;
using PersonalblogServices;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using X.PagedList;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Bmp;
using Personalblog.Model;
using Personalblog.Model.Photography;
//using ImageMagick;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalblogServices.Response;

namespace Personalblog.Services
{
    public class PhotoService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IPhotoService _photoService;
        private readonly MyDbContext _myDbContext;
        public PhotoService(IWebHostEnvironment environment,IPhotoService photoService,MyDbContext myDbContext)
        {
            _environment = environment;
            _photoService = photoService;   
            _myDbContext = myDbContext;
        }
        public async Task<IPagedList<Photo>> GetPageList(int page = 1,int pageSize = 10)
        {
            return (await _photoService.GetAllPhotos()).ToPagedList(page, pageSize);
        }
        public Photo GetRandomPhoto()
        {
            return _photoService.GetRandomPhoto();
        }
        public Photo? GetById(string id)
        {
            return _photoService.GetPhoto(id);
        }
        /// <summary>
        /// 获取图片的物理存储路径
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private string GetPaotoFilePath(Photo photo)
        {
            return Path.Combine(_environment.WebRootPath,"media",photo.FilePath);
        }
        /// <summary>
        /// 重建图片数据（扫描图片的大小等数据）
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private Photo BuildPhotoData(Photo photo)
        {
            var savaPath = GetPaotoFilePath(photo);
            using (var img = SixLabors.ImageSharp.Image.Load(savaPath))
            {
                photo.Height = img.Height;
                photo.Width = img.Width;
            }
            return photo;
        }
        /// <summary>
        /// 批量导入图片
        /// </summary>
        /// <returns></returns>
        public List<Photo> BatchImport()
        {
            var photos = new List<Photo>();
            var importPath = Path.Combine(_environment.WebRootPath, "assets", "photography");
            var root = new DirectoryInfo(importPath);
            foreach (var file in root.GetFiles())
            {
                var photoId = GuidUtils.GuidTo16String();
                var filename = Path.GetFileNameWithoutExtension(file.Name);
                var photo = new Photo()
                {
                    Id = photoId,
                    Title = filename,
                    CreateTime = DateTime.Now,
                    Location = filename,
                    FilePath = Path.Combine("photofraphy", $"{photoId}.jpg")
                };
                var savePath = GetPaotoFilePath(photo);
                file.CopyTo(savePath, true);
                photo = BuildPhotoData(photo);
                _photoService.InsertPhoto(photo);
                photos.Add(photo);
            }
            return photos;
        }
        /// <summary>
        /// 批量导入压缩版
        /// </summary>
        /// <returns></returns>
        public List<Photo> Import()
        {
            var photos = new List<Photo>();
            var importPath = Path.Combine(_environment.WebRootPath, "assets", "yasuo");
            var root = new DirectoryInfo(importPath);
            Task task = Task.Run(() =>
            {
                foreach (var file in root.GetFiles())
                {
                    var photoId = GuidUtils.GuidTo16String();
                    var filename = Path.GetFileNameWithoutExtension(file.Name);
                    var photo = new Photo()
                    {
                        Id = photoId,
                        Title = filename,
                        CreateTime = DateTime.Now,
                        Location = filename,
                        FilePath = Path.Combine("yasuo", $"{photoId}.jpg")
                    };
                    var savePath = GetPaotoFilePath(photo);

                    GetPicThumbnailTest(importPath + "\\" + filename + ".jpg", savePath, Convert.ToInt32(photo.Height), Convert.ToInt32(photo.Width), 50);

                    //file.CopyTo(savePath, true);
                    photo = BuildPhotoData(photo);
                    _photoService.InsertPhoto(photo);
                    Console.WriteLine($"{photo.Id}添加成功！");
                    photos.Add(photo);
                }
            });
            task.Wait();
            return photos;
        }
        /// <summary>
        /// 批量导入压缩全平台版本
        /// </summary>
        /// <returns></returns>
        public List<Photo> Import2()
        {
            var photos = new List<Photo>();
            var importPath = Path.Combine(_environment.WebRootPath, "assets", "yasuo2");
            var root = new DirectoryInfo(importPath);
            Task task = Task.Run(() =>
            {
                foreach (var file in root.GetFiles())
                {
                    var photoId = GuidUtils.GuidTo16String();
                    var filename = Path.GetFileNameWithoutExtension(file.Name);
                    var photo = new Photo()
                    {
                        Id = photoId,
                        Title = filename,
                        CreateTime = DateTime.Now,
                        Location = filename,
                        FilePath = Path.Combine("yasuo2", $"{photoId}.jpg")
                    };
                    var savePath = GetPaotoFilePath(photo);

                    //CompressImage(importPath + "\\" + filename + ".jpg", savePath, Convert.ToInt32(photo.Height), Convert.ToInt32(photo.Width));

                    //file.CopyTo(savePath, true);
                    photo = BuildPhotoData(photo);
                    _photoService.InsertPhoto(photo);
                    Console.WriteLine($"{photo.Id}添加成功！");
                    photos.Add(photo);
                }
            });
            task.Wait();
            return photos;
        }
        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="sFile">原图片位置</param>
        /// <param name="dFile">压缩后图片位置</param>
        /// <param name="dHeight">图片压缩后的高度</param>
        /// <param name="dWidth">图片压缩后的宽度</param>
        /// <param name="flag">图片压缩比0-100,数值越小压缩比越高，失真越多</param>
        /// <returns></returns>
        public static bool GetPicThumbnailTest(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            //如果为参数为0就保持原图片的高宽嘛（不然想保持原图外面还要去读取一次）
            if (dHeight == 0)
            {
                dHeight = iSource.Height;
            }
            if (dWidth == 0)
            {
                dWidth = iSource.Width;
            }


            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            System.Drawing.Size tem_size = new System.Drawing.Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(System.Drawing.Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new System.Drawing.Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
        public Photo Add(PhotoCreationDto dto, IFormFile photoFile)
        {
            var photoId = GuidUtils.GuidTo16String();
            var photo = new Photo
            {
                Id = photoId,
                Title = dto.Title,
                CreateTime = DateTime.Now,
                Location = dto.Location,
                FilePath = Path.Combine("photofraphy", $"{photoId}.jpg")
            };
            var savePath = GetPaotoFilePath(photo);

            const int maxWidth = 2000;
            const int maxHeight = 2000;
            using (var image = SixLabors.ImageSharp.Image.Load(photoFile.OpenReadStream()))
            {
                if (image.Width > maxWidth)
                    image.Mutate(a => a.Resize(maxWidth, 0));
                if (image.Height > maxHeight)
                    image.Mutate(a => a.Resize(0, maxHeight));
                image.Save(savePath);
            }

            using (var fs = new FileStream(savePath, FileMode.Create))
            {
                photoFile.CopyTo(fs);
            }
            string yasuoPath = Path.Combine(_environment.WebRootPath, Path.Combine("media/yasuo2", $"{photoId}.jpg"));
            //压缩图片
            CompressImage(savePath, yasuoPath, 30);
            photo.YPath = Path.Combine("yasuo2", $"{photoId}.jpg");

            photo = BuildPhotoData(photo);
            try
            {
                _myDbContext.Add(photo);
                _myDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return photo;
        }
        /// <summary>
        /// 生成Progressive JPEG缩略图 （使用 MagickImage）
        /// </summary>
        //public async Task<byte[]> GetThumb(string id, int width = 300)
        //{
        //    var photo = await _myDbContext.photos.Where(a => a.Id == id).FirstAsync();
        //    using (var image = new MagickImage(GetPaotoFilePath(photo)))
        //    {
        //        image.Format = MagickFormat.Pjpeg;
        //        image.Resize(width, 0);
        //        return image.ToByteArray();
        //    }
        //}
        /// <summary>
        /// 添加推荐图片
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        public FeaturedPhoto AddFeaturedPhoto(Photo photo)
        {
            var item = _myDbContext.featuredPhotos.FirstOrDefault(a => a.PhotoId == photo.Id);
            if (item != null) return item;
            item = new FeaturedPhoto { PhotoId = photo.Id };
            _myDbContext.featuredPhotos.Add(item);
            _myDbContext.SaveChanges();
            return item;
        }
        /// <summary>
        /// 删除推荐图片
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        public int DeleteFeaturedPhoto(Photo photo)
        {
            var item = _myDbContext.featuredPhotos.FirstOrDefault(a => a.PhotoId == photo.Id);
            if (item == null) return 0;
            _myDbContext.featuredPhotos.Remove(item);
            _myDbContext.SaveChanges();
            return 1;
        }
        /// <summary>
        /// 删除照片
        /// <para>删除照片文件和数据库记录</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteById(string id)
        {
            var photo = _myDbContext.photos.FirstOrDefault(a => a.Id == id);
            var fPhoto = _myDbContext.featuredPhotos.FirstOrDefault(a => a.PhotoId == id);
            if (photo == null) return -1;
            var filePath = GetPaotoFilePath(photo);
            if (File.Exists(filePath)) File.Delete(filePath);
            _myDbContext.photos.Remove(photo);
            if(fPhoto != null) _myDbContext.featuredPhotos.Remove(fPhoto);
            _myDbContext.SaveChanges();
            return 1;
        }
        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="inputImagePath">原图片路径</param>
        /// <param name="outputDirectory">输出目录</param>
        /// <param name="quality">压缩质量</param>
        public static void CompressImage(string inputImagePath, string outputDirectory, int quality)
        {
            // 加载原始图片
            using var image = SixLabors.ImageSharp.Image.Load(inputImagePath);

            // 设置压缩选项
            var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
            {
                Quality = quality
            };

            // 生成输出图片路径
            string fileName = Path.GetFileName(inputImagePath);
            //string outputImagePath = Path.Combine(outputDirectory, fileName);

            // 保存压缩后的图片
            using var outputStream = new FileStream(outputDirectory, FileMode.Create);
            image.Save(outputStream, encoder);
        }
        public async Task<Photo?> GetNext(string id)
        {
            var photo = await  _myDbContext.photos.Where(a=>a.Id == id).FirstOrDefaultAsync();
            if (photo == null) return null;
            var next = await _myDbContext.photos.
                Where(a=>a.CreateTime < photo.CreateTime && a.Id != id).
                OrderByDescending(a=>a.CreateTime).FirstOrDefaultAsync();
            return next;
        }
        public async Task<Photo?> GetPrevious(string id)
        {
            var photo = await _myDbContext.photos.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (photo == null) return null;
            var next = await _myDbContext.photos
                .Where(a => a.CreateTime > photo.CreateTime && a.Id != id)
                .OrderBy(a => a.CreateTime)
                .FirstOrDefaultAsync();
            return next;
        }
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片地址</param>
        /// <param name="dFile">压缩后保存图片地址</param>
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <returns></returns>
        #region
        //public static bool CompressImage(string sFile, string dFile, int height, int width)
        //{
        //    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(sFile))
        //    {
        //        if (height == 0)
        //        {
        //            height = image.Height;
        //        }
        //        if (width == 0)
        //        {
        //            width = image.Width;
        //        }
        //        // Resize the image in place and return it for chaining.
        //        // 'x' signifies the current image processing context.
        //        image.Mutate(x => x.Resize(width, height));

        //        // The library automatically picks an encoder based on the file extensions then encodes and write the data to disk.
        //        image.Save(dFile);
        //        return true;
        //    }
        //}
        #endregion
    }
}
