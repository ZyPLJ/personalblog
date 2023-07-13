using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Categories;
using Personalblog.Model.ViewModels.QueryFilters;
using PersonalblogServices.Response;

namespace PersonalblogServices.CommentService;

public interface Icommentservice
{
    //提交评论
    Task<ApiResponse> SubmitCommentsAsync(Comment comments);
    //回复评论
    Task<ApiResponse> ReplyCommentsAsync(Comment comments);
    //根据ParentCommentId去查找评论收件人邮箱
    Task<string> GetEmail(string ParentCommentId);
    //根据CommentId删除评论
    Task<ApiResponse> DelCommentAsync(string id);
    //根据文章查询该篇文章下是所有评论 分页查询
    Task<(List<Comment>, PaginationMetadata)> GetPagedList(CommentQueryParameter param);
    //添加评论
    Task<Comment> Add(Comment comment);
    //创建评论用户
    Task<AnonymousUser> GetOrCreateAnonymousUser(string name, string email, string? url, string? ip);

    Task<ApiResponse> RangeDelAsync(List<string> id);
}