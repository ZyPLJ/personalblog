namespace Personalblog.Extensions.SendEmail;



public abstract class EmailTemplateBase
{
    protected abstract string GetStyle();
}

public abstract class EmailTemplate:EmailTemplateBase
{
    public string GenerateContent(EmailContent emailContent)
    {
        return $"<html>\n<head>\n<style>{GetStyle()}</style>\n</head>\n<body>\n{GetBodyContent(emailContent)}\n</body>\n</html>";
    }
    protected abstract string GetBodyContent(EmailContent emailContent);
}

public abstract class SEmailTemplate<T>:EmailTemplateBase
{
    public string GenerateContent(EmailContent<T> emailContent)
    {
        return $"<html>\n<head>\n{GetStyle()}\n</head>\n<body>\n{GetBodyContent(emailContent)}\n</body>\n</html>";
    }
    
    protected abstract string GetBodyContent(EmailContent<T> emailContent);
}
