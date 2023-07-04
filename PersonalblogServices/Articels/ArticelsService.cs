﻿using AutoMapper;
using Markdig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.Extensions.Markdown;
using Personalblog.Model.ViewModels;
using X.PagedList;

namespace PersonalblogServices.Articels
{
    public class ArticelsService : IArticelsService
    {
        private readonly MyDbContext _myDbContext;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _generator;
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;
        public ArticelsService(MyDbContext myDbContext, IMapper mapper,
            LinkGenerator linkGenerator, IHttpContextAccessor accessor,
            IConfiguration configuration)
        {
            _myDbContext = myDbContext;
            _mapper = mapper;
            _generator = linkGenerator;
            _accessor = accessor;
            _configuration = configuration;
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

        public async Task<Post> GetArticels(string pid)
        {
            Post post =await _myDbContext.posts.Include("Categories").FirstAsync(p => p.Id == pid);
            post.ViewCount++;
            _myDbContext.posts.Update(post);
            await _myDbContext.SaveChangesAsync();
            return post;
        }

        public IPagedList<Post> GetPagedList(QueryParameters param)
        {
            if (param.CategoryId != 0)
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
        

        public async Task<PostViewModel> GetPostViewModel(Post post)
        {
            
            var model = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Summary = post.Summary ?? "（没有介绍）",
                Content = post.Content ?? "（没有内容）",
                Path = post.Path ?? string.Empty,
                Url = _generator.GetUriByAction(
        _accessor.HttpContext!,
        "Post", "Blog",
        new { post.Id }, "https"
    ),
                CreationTime = post.CreationTime,
                LastUpdateTime = post.LastUpdateTime,
                ViewCount = post.ViewCount,
                Category = post.Categories,
                Categories = new List<Category>(),
                TocNodes = post.ExtractToc()
            };
            // 异步获取 CommentsList
            model.CommentsList = await _myDbContext.comments.Where(c => c.PostId == post.Id).ToListAsync();

            // 异步获取 ConfigItem
            model.ConfigItem = await _myDbContext.configItems.FirstOrDefaultAsync();
            
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            model.ContentHtml = Markdown.ToHtml(model.Content, pipeline);
            return model;
        }

        public async Task<List<Post>> FirstLastPostAsync()
        {
            var currentDate = DateTime.Now; // 获取当前日期
            var targetDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1); // 获取目标日期，即当前月份的上一个月份
            var posts = await _myDbContext.posts
                .Include(p => p.Categories)
                .Include(p => p.Comments)
                .Where(p => p.LastUpdateTime >= targetDate)
                .OrderBy(p => p.CreationTime)
                .ToListAsync();
            var firstPost = posts.FirstOrDefault();
            var lastpost = posts.LastOrDefault();
            return new List<Post> { firstPost, lastpost };
        }

        public async Task<Post> MaxPostAsync()
        {
            return await _myDbContext.posts.OrderByDescending(p => p.ViewCount).FirstOrDefaultAsync();
        }
    }
}
