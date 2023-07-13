using System.ComponentModel.DataAnnotations;

namespace Personalblog.Model.ViewModels.Blog;

public class CommentCreationDto
{
    public string? ParentId { get; set; }

    [Required]
    public string PostId { get; set; }

    [MinLength(2, ErrorMessage = "长度在 2 到 20 个字符")]
    [MaxLength(20, ErrorMessage = "长度在 2 到 20 个字符")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "邮箱地址不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; }

    [Url(ErrorMessage = "请输入正确的url")]
    public string? Url { get; set; }

    [Required(ErrorMessage = "评论内容不能为空")]
    [MaxLength(300, ErrorMessage = "评论最大长度为300个字符")]
    public string Content { get; set; }
}