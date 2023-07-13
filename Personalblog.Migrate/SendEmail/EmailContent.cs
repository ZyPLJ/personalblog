namespace Personalblog.Extensions.SendEmail;

public class EmailContent
{
    public string Content { get; set; }
    public string Name { get; set; }
    public string? Link { get; set; }
}

public class EmailContent<T>
{
    public string Content { get; set; }
    public string Link { get; set; }
    public T Data { get; set; }
}
