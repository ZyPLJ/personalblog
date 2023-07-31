using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.FtopPost
{
    public interface ITopPostService
    {
        // Post GetTopOnePost();
        Task<List<Post>> GetTopOnePostAsync();
    }
}
