using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.QueryFilters;
using PersonalblogServices.Response;
using X.PagedList;

namespace PersonalblogServices.CommentService;

public interface Icommentservice
{
    //提交评论
    Task<ApiResponse> SubmitCommentsAsync(Comments comments);
    //回复评论
    Task<ApiResponse> ReplyCommentsAsync(Comments comments);
    //根据ParentCommentId去查找评论收件人邮箱
    Task<string> GetEmail(int ParentCommentId);
    //分页查询所有评论 可以根据内容模糊查询
    IPagedList<Comments> GetPagedCommentlist(CommentQueryParameter param);
    //更据CommentId删除评论
    Task<ApiResponse> DelCommentAsync(int CId);
}