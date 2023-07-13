using Personalblog.Model.ViewModels;

namespace Personalblog.Model.Entitys;

public class AnonymousUser:ModelBase
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Url { get; set; }
    public string? Ip { get; set; }
}