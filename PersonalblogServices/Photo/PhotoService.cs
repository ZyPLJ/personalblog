using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Personalblog.Migrate;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.Photography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace PersonalblogServices
{
    public class PhotoService : IPhotoService
    {
        private readonly MyDbContext _myDbContext;
        public PhotoService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }


        public async Task<List<Photo>> GetAllPhotos()
        {
            return await _myDbContext.photos.OrderByDescending(p => p.CreateTime).ToListAsync();
        }

        public IPagedList<Photo> GetPagedList(int page = 1, int pageSize = 10)
        {
            return _myDbContext.photos.ToList().ToPagedList(page, pageSize);
        }

        public Photo GetPhoto(string id)
        {
            return _myDbContext.photos.FirstOrDefault(p => p.Id == id);
        }

        public Photo GetRandomPhoto()
        {
            var items = _myDbContext.photos.ToList();
            return items.Count == 0 ? null : items[Random.Shared.Next(items.Count)];
        }

        public void InsertPhoto(Photo photo)
        {
            _myDbContext.Entry<Photo>(photo).State = EntityState.Added;
            int result = _myDbContext.SaveChanges();
            if(result > 0)
            {
                Console.WriteLine("添加图片成功！");
            }
        }
    }
}
