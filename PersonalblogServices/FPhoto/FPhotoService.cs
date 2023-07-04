using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.FPhoto
{
    public class FPhotoService : IFPhotoService
    {
        private readonly MyDbContext _myDbContext;
        public FPhotoService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public List<Photo> GetFeaturePhotos()
        {
            var fp = _myDbContext.featuredPhotos.Include("Photo");
            List<Photo> photos = new List<Photo>();
            foreach (var p in fp)
            {
                photos.Add(p.Photo);
            }
            return photos;
        }
    }
}
