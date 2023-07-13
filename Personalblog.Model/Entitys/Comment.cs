using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Personalblog.Model.ViewModels;

namespace Personalblog.Model.Entitys;

/// <summary>
/// 评论表
/// </summary>
public class Comment:ModelBase
{
    [Key] //主键
    public string Id { get; set; }
    /// <summary>
    /// 父评论id
    /// </summary>
    public string? ParentId { get; set; }
    public Comment? Parent { get; set; }
    public List<Comment>? Comments { get; set; }
    /// <summary>
    /// 文章id
    /// </summary>
    public string PostId { get; set; }
    public  Post Post { get; set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public string? UserId { get; set; }
    public User? User { get; set; }
    
    /// <summary>
    /// 普通用户
    /// </summary>
    public string? AnonymousUserId { get; set; }
    public AnonymousUser? AnonymousUser { get; set; }
    
    //请求头信息
    public string? UserAgent { get; set; }
    //内容
    public string Content { get; set; }
    // todo 评论是否可见，暂时不做
    // public bool Visible { get; set; }
    // 如果验证不通过的话，可能会附上原因
    // public string? Reason { get; set; }
}