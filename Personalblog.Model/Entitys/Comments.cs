using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personalblog.Model.Entitys;

/// <summary>
/// 评论表
/// </summary>
public class Comments
{
    [Key] //主键
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增
    public int CommentId { get; set; }
    /// <summary>
    /// 评论所属文章的ID（外键）
    /// </summary>
    public string PostId  { get; set; }
    /// <summary>
    /// 父评论的ID（用于回复评论）
    /// </summary>
    public int ParentCommentId { get; set; }
    /// <summary>
    /// 评论者的名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 评论者的邮箱
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// 评论的内容
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    /// 评论的创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}