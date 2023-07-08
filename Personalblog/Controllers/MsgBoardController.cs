using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Personalblog.Migrate;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.MessageBoard;
using PersonalblogServices.Messages;
using PersonalblogServices.Response;
using X.PagedList;

namespace Personalblog.Controllers;

public class MsgBoardController : Controller
{
    private readonly IMessagesService _messagesService;

    public MsgBoardController(IMessagesService messagesService)
    {
        _messagesService = messagesService;
    }
    // GET
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        MsgBoardList msgBoardList = new MsgBoardList()
        {
            PagedList = _messagesService.GetAll(new QueryParameters
            {
                Page = page,
                PageSize = pageSize
            }),
            MessagesList = await _messagesService.GetAllasync(),
        };
        return View(msgBoardList);
    }

    /// <summary>
    /// 新增留言
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse> SubMessage([FromBody]Messages messages)
    {
        if(messages.Message == null || messages.Message == "")
            return new ApiResponse(){Data = "请输入留言内容",Message = "请输入留言内容",StatusCode = 422};
        if(messages.Name == null || messages.Name == "")
            return new ApiResponse(){Message = "请输入昵称~",StatusCode = 422};
        if (messages.Email == null || messages.Email == "")
            return new ApiResponse(){Message = "请输入邮箱~",StatusCode = 422};
        bool isValid = CheckEmail.CheckEmailFormat(messages.Email);
        if (!isValid)
        {
            return new ApiResponse(){Message = "邮箱格式错误~",StatusCode = 422};
        }
        try
        {
            return new ApiResponse(){Data = GetHtml(await _messagesService.SubmitMessageAsync(messages)),Message = "留言成功",StatusCode = 200};
        }
        catch (Exception e)
        {
            return new ApiResponse() { Data = "服务器异常！", Message = "服务器异常！", StatusCode = 500 };
        }
    }

    [HttpPost]
    public async Task<ApiResponse> ReplyMessage([FromBody]Replies replies,string replyEmail)
    {
        if(replies.Reply == null || replies.Reply == "")
            return new ApiResponse(){Data = "请输入留言内容",Message = "请输入留言内容",StatusCode = 422};
        if(replies.Name == null || replies.Name == "")
            return new ApiResponse(){Message = "请输入昵称~",StatusCode = 422};
        if (replies.Email == null || replies.Email == "")
            return new ApiResponse(){Message = "请输入邮箱~",StatusCode = 422};
        bool isValid = CheckEmail.CheckEmailFormat(replies.Email);
        if (!isValid)
        {
            return new ApiResponse(){Message = "邮箱格式错误~",StatusCode = 422};
        }

        var data = GetHtml(await _messagesService.ReplyMessageAsync(replies));
        try
        {
            await _messagesService.SendEmailOnAdd(replyEmail, data);
            // SendEmail(replyEmail, data);
        }
        catch (Exception e)
        {
            return new ApiResponse() { Data = "邮箱错误，请检查邮箱格式或联系站长！", Message = "邮箱错误，请检查邮箱格式或联系站长！", StatusCode = 422 };
        }
        try
        {
            return new ApiResponse()
            {
                Data = data, Message = "回复留言成功", StatusCode = 200
            };
        }
        catch (Exception e)
        {
            return new ApiResponse() { Data = "服务器异常！", Message = "服务器异常！", StatusCode = 500 };
        }
    }
    
    public string GetHtml(Messages messages)
    {
        return GetToHtml.GetFeedbackHtml(messages.Name, messages.Message, messages.created_at);
    }

    public string GetHtml(Replies replies)
    {
        return GetToHtml.GetFeedbackHtml(replies.Name, replies.Reply, replies.created_at);
    }
}