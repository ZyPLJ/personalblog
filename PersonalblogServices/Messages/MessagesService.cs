using System.Text;
using Microsoft.EntityFrameworkCore;
using Personalblog.Extensions.SendEmail;
using Personalblog.Extensions.SendEmail.Services;
using Personalblog.Migrate;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using PersonalblogServices.Response;
using Msg = Personalblog.Model.Entitys.Messages;
using X.PagedList;

namespace PersonalblogServices.Messages;

public class MessagesService:IMessagesService
{
    private readonly MyDbContext _myDbContext;
    private IEmailService emailService;
    private readonly EmailServiceFactory _emailServiceFactory;
    public MessagesService(MyDbContext myDbContext,EmailServiceFactory emailServiceFactory)
    {
        _myDbContext = myDbContext;
        _emailServiceFactory = emailServiceFactory;
    }
    public async Task<Personalblog.Model.Entitys.Messages> SubmitMessageAsync(Personalblog.Model.Entitys.Messages messages)
    {
        StringBuilder sb = CommentSJson.CommentsJson(messages.Message);
        messages.Message = sb.ToString();
        messages.created_at = DateTime.Now;
        await _myDbContext.Messages.AddAsync(messages);
        await _myDbContext.SaveChangesAsync();
        return messages;
    }

    public IPagedList<Personalblog.Model.Entitys.Messages> GetAll(QueryParameters param)
    {
        return _myDbContext.Messages.Include(m => m.Replies).ToList().ToPagedList(param.Page, param.PageSize);
        // return _myDbContext.Messages.ToList().ToPagedList(param.Page, param.PageSize);
    }

    public async Task<List<Personalblog.Model.Entitys.Messages>> GetAllasync()
    {
        return await _myDbContext.Messages.ToListAsync();
    }

    public async Task<ApiResponse> DelMessageAsync(int id)
    {
        try
        {
            using (var transaction = await _myDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = new Msg() { Id = id };
                    _myDbContext.Messages.Attach(msg);
                    _myDbContext.Messages.Remove(msg);

                    var replies = await _myDbContext.Replies.Where(r => r.MessageId == id).ToListAsync();
                    _myDbContext.Replies.RemoveRange(replies);

                    await _myDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ApiResponse { Message = $"留言{id}删除成功" };
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine(e);
                    return new ApiResponse { Message = "删除失败！", StatusCode = 500 };
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ApiResponse { Message = "删除失败！", StatusCode = 500 };
        }
    }

    public async Task<Replies> ReplyMessageAsync(Replies replies)
    {
        StringBuilder sb = CommentSJson.CommentsJson(replies.Reply);
        replies.Reply = sb.ToString();
        replies.created_at = DateTime.Now;
        await _myDbContext.Replies.AddAsync(replies);
        await _myDbContext.SaveChangesAsync();
        return replies;
    }

    public async Task<List<Replies>> GetMsgReplyAsync()
    {
        return await _myDbContext.Replies.ToListAsync();
    }

    public IPagedList<Replies> GetAllReply(QueryParameters param)
    {
        return _myDbContext.Replies.ToList().ToPagedList(param.Page, param.PageSize);
    }

    public async Task<ApiResponse> DelMessageReplyAsync(int id)
    {
        try
        {
            var replies = new Replies(){Id = id};
            _myDbContext.Replies.Attach(replies);
            _myDbContext.Replies.Remove(replies);
            await _myDbContext.SaveChangesAsync();
            return new ApiResponse { Message = $"留言{id}删除成功"};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ApiResponse { Message = "删除失败！", StatusCode = 500 };
        }
    }

    public async Task SendEmailOnAdd(string email, string content)
    {
        emailService = await _emailServiceFactory.CreateEmailService();
        var template = new MessageBoardNotificationEmailTemplate();
        await emailService.SendEmail(email, template,
            new EmailContent() { Content = content });
    }
}