namespace Personalblog.Extensions.SendEmail;

public class CommentNotificationEmailTemplate:EmailTemplate
{
    protected override string GetStyle()
    {
        return "";
    }

    protected override string GetBodyContent(EmailContent emailContent)
    {
        return $"您收到来自ZY知识库评论通知，" +
               $"<br>内容如下：{emailContent.Content}" +
               $"<br>评论者：{emailContent.Name}" +
               $"<br>点击跳转：" +
               $"<a href='https://pljzy.top/blog/post/{emailContent.Link}.html'>文章地址</a>";
    }
}