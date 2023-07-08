using System.Text;
using Personalblog.Model.Entitys;

namespace Personalblog.Extensions.SendEmail;

public class LinksNotificationEmailTemplate:SEmailTemplate<LinkExchange>
{
    protected override string GetStyle()
    {
        return "";
    }

    protected override string GetBodyContent(EmailContent<LinkExchange> emailContent)
    {
        const string blogLink = "<a href=\"https://pljzy.top\">ZY知识库</a>";
        var sb = new StringBuilder();
        sb.AppendLine($"<p>{emailContent.Content}</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>以下是您申请的友链信息：</p>");
        sb.AppendLine($"<p>网站名称：{emailContent.Data.Name}</p>");
        sb.AppendLine($"<p>介绍：{emailContent.Data.Description}</p>");
        sb.AppendLine($"<p>网址：{emailContent.Data.Url}</p>");
        sb.AppendLine($"<p>站长：{emailContent.Data.WebMaster}</p>");
        sb.AppendLine($"<p>补充信息：{emailContent.Data.Reason}</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>本消息由 {blogLink} 自动发送，无需回复。</p>");
        return sb.ToString();
    }
}