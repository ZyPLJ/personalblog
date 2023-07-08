using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Personalblog.Extensions.SendEmail.Services;
using Personalblog.Migrate.SendEmail.Services;
using Personalblog.Model.ViewModels;

namespace Personalblog.Extensions.SendEmail;

public class EmailServiceFactory
{
    private readonly EmailConfig emailConfig;
    public EmailServiceFactory(IOptions<EmailConfig> emailConfigOptions)
    {
        emailConfig = emailConfigOptions.Value;
    }
    public async Task<IEmailService> CreateEmailService()
    {
        var client = new SmtpClient();
        // configure the client
        await client.ConnectAsync("smtp.163.com", 465, true);
        await client.AuthenticateAsync(emailConfig.Address, emailConfig.Password);

        return new MimeKitEmailService(client);
    }
}