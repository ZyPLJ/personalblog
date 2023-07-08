using MailKit.Net.Smtp;
using MimeKit;
using Personalblog.Extensions.SendEmail;
using Personalblog.Extensions.SendEmail.Services;

namespace Personalblog.Migrate.SendEmail.Services;

public class MimeKitEmailService : IEmailService
{
    private readonly SmtpClient client;

    public MimeKitEmailService(SmtpClient client)
    {
        this.client = client;
    }

    public async Task SendEmail(string recipient, EmailTemplate template, EmailContent emailContent)
    {
        var message = CreateEmailMessage(recipient, template.GenerateContent(emailContent));
        await SendAsync(message);
    }

    public async Task SendEmail<T>(string recipient, SEmailTemplate<T> template, EmailContent<T> emailContent)
    {
        var message = CreateEmailMessage(recipient, template.GenerateContent(emailContent));
        await SendAsync(message);
    }

    private MimeMessage CreateEmailMessage(string recipient, string content)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("ZY", "zy1767992919@163.com"));
        message.To.Add(new MailboxAddress("", recipient));
        message.Subject = "来自ZY知识库通知~";
        message.Body = new TextPart("html")
        {
            Text = content
        };

        return message;
    }

    private async Task SendAsync(MimeMessage message)
    {
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
