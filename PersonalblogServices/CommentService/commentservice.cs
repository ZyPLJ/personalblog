using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Personalblog.Migrate;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.QueryFilters;
using PersonalblogServices.Response;
using X.PagedList;

namespace PersonalblogServices.CommentService;

public class commentservice:Icommentservice
{
    private readonly MyDbContext _dbContext;

    public commentservice(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ApiResponse> SubmitCommentsAsync(Comments comments)
    {
        try
        {
            StringBuilder sb = CommentSJson.CommentsJson(comments.Content);
            comments.Content = sb.ToString();
            await _dbContext.comments.AddAsync(comments);
            await _dbContext.SaveChangesAsync();
            return new ApiResponse() { StatusCode = 200,Message = $"评论成功！",Data = comments};
        }
        catch (Exception e)
        {
            return new ApiResponse() { StatusCode = 500,Message = $"{e.Message}" };
        }
    }
    public async Task<ApiResponse> ReplyCommentsAsync(Comments comments)
    {
        try
        {
            await _dbContext.comments.AddAsync(comments);
            await _dbContext.SaveChangesAsync();
            return new ApiResponse(){Data = comments};
        }
        catch (Exception e)
        {
            return new ApiResponse() { StatusCode = 500,Message = $"{e.Message}" };
        }
    }

    public async Task<string> GetEmail(int ParentCommentId)
    {
        try
        {
            var data = await _dbContext.comments.FirstOrDefaultAsync(c=>c.CommentId == ParentCommentId);
            return data.Email;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "邮箱错误";
            throw;
        }
    }

    public IPagedList<Comments> GetPagedCommentlist(CommentQueryParameter param)
    {
        var queryset = _dbContext.comments.ToArray();
        
        //模糊查询
        if (!string.IsNullOrEmpty(param.Content))
        {
            queryset = queryset.Where(a => a.Content.Contains(param.Content)).ToArray();
        }

        queryset = queryset.OrderByDescending(a => a.CreateTime).ToArray();

        return queryset.ToList().ToPagedList(param.Page, param.PageSize);
    }

    public async Task<ApiResponse> DelCommentAsync(int CId)
    {
        try
        {
            var comment = new Comments { CommentId = CId };
            _dbContext.comments.Attach(comment);
            _dbContext.comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return new ApiResponse { Message = $"评论{CId}删除成功"};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ApiResponse { Message = "删除失败！", StatusCode = 500 };
            throw;
        }
    }
}