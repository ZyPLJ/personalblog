using System.ComponentModel.DataAnnotations;

namespace Personalblog.Model.ViewModels;

public class InitViewModel
{
    [Display(Name = "用户名")]
    public string Username { get; set; }
    [Display(Name = "密码")]
    public string Password { get; set; }
    [Display(Name = "博客域名")]
    public string Host { get; set; }
}