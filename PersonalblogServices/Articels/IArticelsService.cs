using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using PersonalblogServices.Articels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace PersonalblogServices.Articels
{
    public interface IArticelsService
    {
        //根据文章id查看文章
        ArticelRes GetArticels(string pid);
        //根据文章列表去查询文章
        IPagedList<Post> GetPagedList(QueryParameters param);
        //查询全部文章 随机文章
        List<Post> GetPhotos();
        //添加文章
        Post AddPost(Post post);
    }
}
