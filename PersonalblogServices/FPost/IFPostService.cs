using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.FPost
{
    public interface IFPostService
    {
        List<Post> GetFeaturedPosts();
        FeaturedPost GetFeatures(int id);
        List<FeaturedPost> GetList();
        int Delete(FeaturedPost featured);
    }
}
