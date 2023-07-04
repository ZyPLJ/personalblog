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
            SendEmail(replyEmail, data);
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
    private static void SendEmail(string email, string content)
    {
        try
        {
            // 创建邮件
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ZY", "1767992919@qq.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "ZY知识库评论通知";
            message.Body = new TextPart("html")
            {
                Text = $@"
                    <html>
                        <head>
                            <style>
                                /* 添加样式 */
                               body {{
                                    font-family: Arial, sans-serif;
                                    font-size: 14px;
                                    line-height: 1.5;
                                    color: #333;
                                }}
                                .box {{
                                    background-color:#90F8FF ;
                                    border-radius: 5px;
                                    padding: 20px;
                                }}
                                h1 {{
                                    font-size: 24px;
                                    font-weight: bold;
                                    margin-bottom: 20px;
                                    color: #333;
                                }}
                                p {{
                                    margin-bottom: 10px;
                                    color: #333;
                                }}
                                a {{
                                    color: #333;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='box'>
                                <h1>ZY知识库</h1>
                                <h3>留言板通知</h3>
                                <p>内容如下：{content}</p>
                                <p>点击跳转：<a href='https://pljzy.top/MsgBoard?page=1'>留言板地址</a></p>
                            </div>
                        </body>
                    </html>"
            };

            // 发送邮件
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.qq.com", 465, true);
                client.Authenticate("1767992919@qq.com", "wnfgbdddlsmcbfbj");
                client.Send(message);
                client.Disconnect(true);
            }
            Console.WriteLine("邮件已成功发送！");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
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