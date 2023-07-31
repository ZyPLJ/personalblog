using System.Text;
using Microsoft.EntityFrameworkCore;
using Personalblog.Migrate;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Categories;
using Personalblog.Model.ViewModels.QueryFilters;
using PersonalblogServices.Response;

namespace PersonalblogServices.CommentService;

public class commentservice:Icommentservice
{
    private readonly MyDbContext _dbContext;

    public commentservice(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ApiResponse> SubmitCommentsAsync(Comment comments)
    {
        try
        {
            StringBuilder sb = CommentSJson.CommentsJson(comments.Content);
            comments.Content = sb.ToString();
            await _dbContext.Comments.AddAsync(comments);
            await _dbContext.SaveChangesAsync();
            return new ApiResponse() { StatusCode = 200,Message = $"评论成功！",Data = comments};
        }
        catch (Exception e)
        {
            return new ApiResponse() { StatusCode = 500,Message = $"{e.Message}" };
        }
    }
    public async Task<ApiResponse> ReplyCommentsAsync(Comment comments)
    {
        try
        {
            await _dbContext.Comments.AddAsync(comments);
            await _dbContext.SaveChangesAsync();
            return new ApiResponse(){Data = comments};
        }
        catch (Exception e)
        {
            return new ApiResponse() { StatusCode = 500,Message = $"{e.Message}" };
        }
    }

    public async Task<string> GetEmail(string ParentCommentId)
    {
        try
        {
            var data = await _dbContext.Comments.FirstOrDefaultAsync(c=>c.Id == ParentCommentId);
            return "";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "邮箱错误";
            throw;
        }
    }
    

    public async Task<ApiResponse> DelCommentAsync(string id)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null)
            {
                return new ApiResponse { Message = "评论不存在！", StatusCode = 404 };
            }


            var commentIds = await GetCommentIdsAsync(id);
            commentIds.Add(id);

            var commentsToDelete = await _dbContext.Comments.Where(c => commentIds.Contains(c.Id)).ToListAsync();

            _dbContext.Comments.RemoveRange(commentsToDelete);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return new ApiResponse { Message = $"评论{id}删除成功,并删除其余下{commentIds.Count-1}条评论"};
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e);
            return new ApiResponse { Message = "删除失败！", StatusCode = 500 };
        }
    }
    private async Task<List<string>> GetCommentIdsAsync(string parentId)
    {
        var commentIds = new List<string>();

        var comments = await _dbContext.Comments.Where(c => c.ParentId == parentId).ToListAsync();

        foreach (var comment in comments)
        {
            commentIds.Add(comment.Id);
            commentIds.AddRange(await GetCommentIdsAsync(comment.Id));
        }

        return commentIds;
    }
    /// <summary>
    /// 传入文章id 查询评论
    /// 根据评论事件降序排列
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<(List<Comment>, PaginationMetadata)> GetPagedList(CommentQueryParameter param)
    {
        var querySet =  _dbContext.Comments.AsQueryable();

        if (param.PostId != null)
        {
            querySet = querySet.Where(a => a.PostId == param.PostId);
        }

        if (param.Search != null)
        {
            querySet = querySet.Where(a => a.Content.Contains(param.Search));
        }
        
        var data = await querySet
            .Include(a => a.AnonymousUser)
            .Include(a => a.Parent)
            .ThenInclude(a=>a.AnonymousUser)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .ToListAsync();

        var pagination = new PaginationMetadata()
        {
            PageNumber = param.Page,
            PageSize = param.PageSize,
            TotalItemCount = await querySet.CountAsync()
        };
        return (data, pagination);
    }

    public async Task<Comment> Add(Comment comment)
    {
        comment.Id = GuidUtils.GuidTo16String();
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
        return comment;
    }

    public async Task<AnonymousUser> GetOrCreateAnonymousUser(string name, string email, string? url, string? ip)
    {
        var item = await _dbContext.AnonymousUsers
            .Where(a => a.Email == email).FirstOrDefaultAsync();
        if (item == null)
        {
            item = new AnonymousUser()
            {
                Id = GuidUtils.GuidTo16String(),
                Email = email,
                Name = name,
                Ip = ip,
                Url = url,
                CreatedTime = DateTime.Now
            };
            _dbContext.AnonymousUsers.Add(item);
        }
        else
        {
            item.Name = name;
            item.Ip = ip;
            item.Url = url;
            item.UpdatedTime = DateTime.Now;   
            _dbContext.AnonymousUsers.Update(item);
        }
        await _dbContext.SaveChangesAsync();
        
        return item;
    }
    
    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ApiResponse> RangeDelAsync(List<string> id)
    {
        try
        {
            var commentsToDelete = await _dbContext.Comments
                .Include(c => c.Comments)
                .Where(c => id.Contains(c.Id))
                .ToListAsync();

            int deletedCount = 0;

            foreach (var comment in commentsToDelete)
            {
                deletedCount += await DeleteCommentRecursively(comment);
            }

            await _dbContext.SaveChangesAsync();
            return new ApiResponse { Successful = true, Message = $"删除{deletedCount}条数据" };
        }
        catch (Exception ex)
        {
            return new ApiResponse { Successful = false, Message = ex.Message };
        }
    }

    private async Task<int> DeleteCommentRecursively(Comment comment)
    {
        int count = 1;

        if (comment.Comments != null && comment.Comments.Any())
        {
            foreach (var childComment in comment.Comments.ToList())
            {
                count += await DeleteCommentRecursively(childComment);
            }
        }

        _dbContext.Comments.Remove(comment);
        return count;
    }

}