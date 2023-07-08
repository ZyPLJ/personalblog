namespace Personalblog.Extensions.SendEmail.Services;

public interface IEmailService
{
    Task SendEmail(string recipient, EmailTemplate template, EmailContent emailContent);
    Task SendEmail<T>(string recipient, SEmailTemplate<T> template,  EmailContent<T> emailContent);
}