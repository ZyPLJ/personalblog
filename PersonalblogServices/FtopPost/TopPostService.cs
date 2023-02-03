using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return _myDbContext.topPosts.Include("Post").First().Post;
        }
    }
}
