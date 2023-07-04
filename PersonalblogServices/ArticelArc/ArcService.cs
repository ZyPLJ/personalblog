using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Arc;
using X.PagedList;

namespace PersonalblogServices.ArticelArc;

public class ArcService : IArcService
{
    private readonly MyDbContext _myDbContext;

    public ArcService(MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
    }

    public async Task<IPagedList<ArcPost>> GetAllAsync(QueryParameters param)
    {
        var query = _myDbContext.posts
            .GroupBy(p => new { p.CreationTime.Year, p.CreationTime.Month })
            .Select(g => new ArcPost
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Posts = g.ToList()
            })
        .OrderByDescending(g => g.Year)
        .ThenByDescending(g => g.Month) ;
        
        var result = await query.ToPagedListAsync(param.Page, param.PageSize);
        

        return result;
    }

    public async Task<ArcViewPost> GetViewPostAsync()
    {
        // 获取所有文章
        var posts = await _myDbContext.posts.Include("Comments").ToListAsync();

        // 总浏览量
        var totalViewCount = posts.Sum(p => p.ViewCount);

        // 总评论数
        var totalCommentCount = posts.Sum(p => p.Comments.Count);
        // 分类数量
        var categoryCount =await _myDbContext.categories.CountAsync();
        ArcViewPost arcViewPost = new ArcViewPost()
        {
            PostCount = posts.Count,
            ViewCount = totalViewCount,
            CommentCount = totalCommentCount,
            CateGoryCount = categoryCount
        };
        return arcViewPost;
    }
}