using MailKit.Net.Smtp;
using MimeKit;

namespace PersonalblogServices;

public static class EmailUtils
{
    public static async Task SendEmailAsync(string email, string content)
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
                Text = $"{content}"
            };
            // 发送邮件
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.qq.com", 465, true);
                await client.AuthenticateAsync("1767992919@qq.com", "nebttozhnztwdeeb");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            Console.WriteLine("邮件已成功发送！");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    public static void SendEmail(string email, string content, string link)
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
                Text = $"您收到来自ZY知识库评论通知，内容如下：{content}<br>点击跳转：<a href='https://pljzy.top/blog/post/{link}.html'>文章地址</a>"
            };
            // 发送邮件
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.qq.com", 465, true);
                client.Authenticate("1767992919@qq.com", "nebttozhnztwdeeb");
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
}