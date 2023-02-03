using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
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
    public class ArticelsService : IArticelsService
    {
        private readonly MyDbContext _myDbContext;
        private readonly IMapper _mapper;
        public ArticelsService(MyDbContext myDbContext,IMapper mapper)
        {
            _myDbContext = myDbContext;
            _mapper = mapper;
        }

        public Post AddPost(Post post)
        {
            try
            {
                _myDbContext.posts.Add(post);
                _myDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return post;
        }

        public ArticelRes GetArticels(string pid)
        {
            Post post = _myDbContext.posts.Include("Categories").First(p => p.Id == pid);
            //Post post = _myDbContext.posts.FirstOrDefault(p => p.Id == pid);
            //var c = post.Categories;
            //post.Categories = _myDbContext.categories.First(c => c.Id == post.CategoryId);
            return _mapper.Map<ArticelRes>(post);
        }

        public IPagedList<Post> GetPagedList(QueryParameters param)
        {
            if(param.CategoryId != 0)
            {
                return _myDbContext.posts.Where(p => p.CategoryId == param.CategoryId).ToList().ToPagedList(param.Page, param.PageSize);
            }
            else
            {
                return _myDbContext.posts.ToList().ToPagedList(param.Page, param.PageSize);
            }
        }

        public List<Post> GetPhotos()
        {
            return _myDbContext.posts.ToList();
        }
    }
}
