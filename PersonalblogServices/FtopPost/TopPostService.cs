using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;

namespace PersonalblogServices.FtopPost
{
    public class TopPostService : ITopPostService
    {
        private readonly MyDbContext _myDbContext;
        public TopPostService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public Post GetTopOnePost()
        {
            if (_myDbContext.topPosts.Include("Post").FirstOrDefault() == null)
            {
                return null;
            }
            else
            {
                return _myDbContext.topPosts.Include("Post").First().Post;
            }
        }
    }
}
