using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.FPost
{
    public class FPostService : IFPostService
    {
        private readonly MyDbContext _myDbContext;
        public FPostService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public int Delete(FeaturedPost featured)
        {
            _myDbContext.Remove(featured);
            return _myDbContext.SaveChanges();
        }

        public List<Post> GetFeaturedPosts()
        {
            return _myDbContext.featuredPosts.Include(a => a.Post.Categories).Select(a => a.Post).ToList();
        }

        public FeaturedPost GetFeatures(int id)
        {
            return _myDbContext.featuredPosts.Where(a => a.Id == id)
                .Include(a => a.Post).First();
        }

        public List<FeaturedPost> GetList()
        {
            return _myDbContext.featuredPosts.Include(a => a.Post.Categories).ToList();
        }
    }
}
