using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using PersonalblogServices.Response;
using X.PagedList;
using msg = Personalblog.Model.Entitys.Messages;

namespace PersonalblogServices.Messages;

public interface IMessagesService
{
    //新增留言
    Task<msg> SubmitMessageAsync(msg messages);
    //查询所有留言,分页列表
    IPagedList<msg> GetAll(QueryParameters param);
    //查询所有留言，不分页
    Task<List<msg>> GetAllasync();
    //删除留言
    Task<ApiResponse> DelMessageAsync(int id);
    //回复留言
    Task<Replies> ReplyMessageAsync(Replies replies);
    //查询回复的留言
    Task<List<Replies>> GetMsgReplyAsync();
    //查询所有回复留言,分页列表
    IPagedList<Replies> GetAllReply(QueryParameters param);
    //删除回复留言
    Task<ApiResponse> DelMessageReplyAsync(int id);
    //发送邮件
    Task SendEmailOnAdd(string email, string content);
    
}