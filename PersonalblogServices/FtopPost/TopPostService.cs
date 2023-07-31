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
        

        public async Task<List<Post>> GetTopOnePostAsync()
        {
            var topPosts = await _myDbContext.topPosts.Include(t => t.Post).ToListAsync();

            if (topPosts.Count == 0)
            {
                return null;
            }                
            return topPosts.Select(tp => tp.Post).ToList();
        }
    }
}
