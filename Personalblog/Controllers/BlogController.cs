using System.Text.RegularExpressions;
using AspNetCoreRateLimit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using PersonalblogServices.Articels;
using PersonalblogServices.Categorys;
using Personalblog.Contrib.SiteMessage;
using PersonalblogServices.CommentService;
using MimeKit;
using Newtonsoft.Json;
using Personalblog.Migrate;
using PersonalblogServices.Response;
using StackExchange.Profiling.Internal;
using Messages = Personalblog.Contrib.SiteMessage.Messages;

namespace Personalblog.Controllers
{
    public class BlogController : Controller
    {
        public ICategoryService CategoryService { get; set; }
        public IArticelsService ArticelsService { get; set; }
        private Personalblog.Services.CategoryService _categoryService1 { get; set; }
        private readonly Messages _messages;
        private readonly Icommentservice _commentservice;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        public BlogController(ICategoryService categoryService,IArticelsService articelsService, 
            Messages messages, Personalblog.Services.CategoryService categoryService1,
            Icommentservice commentservice,IMemoryCache cache,
            IConfiguration configuration)
        {
            this.CategoryService = categoryService;
            this.ArticelsService = articelsService;
            _categoryService1 = categoryService1;
            _messages = messages;
            _commentservice = commentservice;
            _cache = cache;
            _configuration = configuration;
        }
        /// <summary>
        /// 所有文章列表
        /// </summary>
        /// <param name="categoryId">文章类别id 默认是.net文章</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">页面最大展示数据的数量</param>
        /// <returns></returns>
        public async Task<IActionResult> List(int categoryId = 1, int page = 1, int pageSize = 5)
        {
            var clist = CategoryService.categories();
            var currentCategory = categoryId == 0 ? clist[0] :await CategoryService.GetById(categoryId);

            if (currentCategory == null)
            {
                _messages.Error($"分类 {categoryId} 不存在！");
                return RedirectToAction(nameof(List));
            }
            if (!currentCategory.Visible)
            {
                _messages.Warning($"分类 {currentCategory.Name} 暂不开放！");
                return RedirectToAction(nameof(List));
            }

            BlogListViewModel blogList = new BlogListViewModel()
            {
                CurrentCategory = categoryId == 0 ? clist[0] : clist.First(c => c.Id == categoryId),
                CurrentCategoryId = categoryId,
                Categories = clist,
                CategoryNodes = _categoryService1.GetNodes(),
                Posts = ArticelsService.GetPagedList(new QueryParameters
                {
                    Page = page,
                    PageSize = pageSize,
                    CategoryId = categoryId,
                })
            };
            return View(blogList);
        }
        /// <summary>
        /// 根据文章id去查看文章
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [Route("blog/post/{id}.html")]
        public async Task<IActionResult> Post(string id)
        {
            try
            {
                //缓存
                var post = await _cache.GetOrCreateAsync($"{id}", async (e) =>
                {
                    //设置缓存过期时间10s钟
                    e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                    Console.WriteLine($"缓存没找到，到数据库中查一查，id={id}");
                    return await ArticelsService.GetArticels(id);
                });
                // var sessionId = HttpContext.Session.Id;
                // // 从缓存中获取当前用户的错误信息
                // if (_cache.TryGetValue("Errors", out Dictionary<string, string> errors) 
                //     && errors.TryGetValue(sessionId, out var message))
                // {
                //     // 如果当前用户有错误信息，则显示警告消息
                //     _messages.Warning(message);
                //     // 从缓存中移除当前用户的错误信息
                //     errors.Remove(sessionId);
                // }
                var codeBlockTheme = _configuration.GetSection("CodeBlockTheme").GetValue<string>("Theme");
                if (codeBlockTheme == "rear-end")
                {
                    return View("PostTwo",await ArticelsService.GetPostViewModel(post));
                }
                return View("Post",await ArticelsService.GetPostViewModel(post));
            }
            catch (Exception)
            {
                return Content(id);
                throw;
            }
        }
        /// <summary>
        /// 随机一篇文章
        /// </summary>
        /// <returns></returns>
        public IActionResult RandomPost()
        {
            var posts = ArticelsService.GetPhotos();
            var randPost = posts[Random.Shared.Next(posts.Count)];
            if (posts.Count == 0)
            {
                _messages.Error("当前没有文章，请先添加文章！");
                return RedirectToAction("Index", "Home");
            }
            _messages.Info($"随机推荐了文章 <b>{randPost.Title}</b> 给你~" +
                      $"<span class='ps-3'><a href=\"{Url.Action(nameof(RandomPost))}\">再来一次</a></span>");
            return RedirectToAction(nameof(Post), new { id = randPost.Id });
        }
        [HttpPost]
        public async Task<ApiResponse> SubComment([FromBody]Comments comments)
        {
            if (comments.ParentCommentId == null)
            {
                comments.ParentCommentId = 0;
            }

            if (comments.Content.Equals("[{\"insert\":\"\\n\"}]") || comments.Content == "" || comments.Content == null)
            {
                return new ApiResponse(){Data = "请输入评论内容",Message = "请输入评论内容",StatusCode = 422};
            }

            if (comments.Name == null || comments.Name == "")
            {
                return new ApiResponse(){Message = "请输入昵称~",StatusCode = 422};
            }

            if (comments.Email ==  null || comments.Email =="")
            {
                return new ApiResponse(){Message = "请输入邮箱~",StatusCode = 422};
            }
            bool isValid = CheckEmail.CheckEmailFormat(comments.Email);
            if (!isValid)
            {
                return new ApiResponse(){Message = "邮箱格式错误~",StatusCode = 422};
            }
            comments.CreateTime = DateTime.Now;
            var data = await _commentservice.SubmitCommentsAsync(comments);
            var content = data.Data as Comments;
            try
            {
                if (comments.ParentCommentId != 0)
                {
                    var email = await _commentservice.GetEmail(comments.ParentCommentId);
                    if (email == "邮箱错误")
                    {
                        return new ApiResponse(){Message = "邮箱错误~",StatusCode = 422};
                    }
            
                    try
                    {
                        await ArticelsService.SendEmailOnAdd(email, content.Content, $"{comments.PostId}");
                        // SendEmail(email, content.Content, $"{comments.PostId}");
                    }
                    catch (Exception e)
                    {
                        return new ApiResponse(){Message = "邮箱错误~",StatusCode = 422};
                    }
                }
            }
            catch (Exception e)
            {
                _messages.Error("邮箱错误！，无法发送邮箱信息~");
                throw;
            }
            return new ApiResponse(){Data = GetHtml(content),StatusCode = 200,Message = "评论成功~"};
        }
        #region MyRegion
        /*
        private static void SendEmail(string email,string content,string link)
        {
            try
            {
                // 设置发件人、收件人、邮件主题和正文
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("1767992919@qq.com", "ZY");
                mail.To.Add(new MailAddress(email));
                mail.Subject = "ZY知识库评论通知";
                mail.Body = $"您收到来自ZY知识库评论通知，内容如下{content}<br>点击跳转:<a href='https://pljzy.top/blog/post/{link}.html'>文章地址</a>";
                mail.IsBodyHtml = true;
                
                // 设置 SMTP 客户端
                SmtpClient client = new SmtpClient("smtp.qq.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("1767992919@qq.com", "nebttozhnztwdeeb");
                // 添加 EHLO 和 AUTH 支持
                client.TargetName = "STARTTLS/smtp.qq.com";
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                // 发送邮件
                try
                {
                    client.Send(mail);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                Console.WriteLine("邮件已成功发送！");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }*/
        #endregion
        #region 废弃发送邮件
        private static void SendEmail(string email, string content, string link)
        {
            try
            {
                // 创建邮件
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ZY", "zy1767992919@163.com"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "ZY知识库评论通知";
                message.Body = new TextPart("html")
                {
                    Text = $"您收到来自ZY知识库评论通知，内容如下：{content}<br>点击跳转：<a href='https://pljzy.top/blog/post/{link}.html'>文章地址</a>"
                };
                // 发送邮件
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.163.com", 465, true);
                    client.Authenticate("zy1767992919@163.com", "TYRTKMDEPQMOXXKG");
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
        #endregion
        
        public string GetHtml(Comments comments)
        {
            string html = $@"
        <div class=""feedbackItem"">
            <div class=""feedbackListSubtitle feedbackListSubtitle-louzhu"">
                <div class=""feedbackManage"">
                    <span class=""comment_actions"">
                        <a class=""comment_actions_link"" href=""#reply"" onclick=""Reply({comments.CommentId},'{comments.Name}')"" id=""Reply"">回复</a>
                    </span>
                </div>
                <span class=""comment_date"">{comments.CreateTime}</span>
                <span class=""a_comment_author_5166961"">{comments.Name}</span>
            </div>
            <div class=""feedbackCon"">
                {comments.Content}
            </div>   
        </div>";
            return html;
        }
    }
}
